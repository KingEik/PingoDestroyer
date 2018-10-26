using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace PingoDestroyer
{
    public partial class MainForm : Form
    {
        private Helper connection;

        private String baseUrl = "http://pingo.upb.de/";
        private int sessionID = -1;

        Uri voteTarget;
        Uri currentTarget;
        bool working = false;

        int threadCount = 30;
        int waitMillis = 200;
        List<Thread> workers = new List<Thread>();

        int votesGiven = 0;
        String statusText;

        List<int> forceInput = new List<int>();

        private Thread refresher;
        private Label status;

        public MainForm()
        {
            Logger.debug("Initialising...");
            InitializeComponent();
            voteTarget = new Uri(baseUrl + "vote");
            status = new Label();
            status.AutoSize = true;
            status.Location = new Point(15, 15);
            connection = new Helper();
            connection.acceptCookies = false;
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (refresher != null && refresher.IsAlive)
                refresher.Abort();
            stopGivingAnswers();
            int sid;
            if (int.TryParse(tb_sessionID.Text, out sid))
            {
                this.sessionID = sid;
                lbl_activeID.Text = this.sessionID.ToString();

                this.threadCount = Decimal.ToInt32(nud_threads.Value);
                this.waitMillis = Decimal.ToInt32(nud_waitTime.Value);

                this.forceInput = new List<int>();
                StringBuilder sb = new StringBuilder();
                String[] splits = tb_forceInput.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (String s in splits)
                {
                    int v;
                    if (int.TryParse(s, out v)) {
                        this.forceInput.Add(v);
                        sb.Append(v).Append(';');
                    }
                }

                if (sb.Length > 0)
                    sb.Length--;
                else
                    sb.Append("Random");

                lbl_forceInput.Text = sb.ToString();

                updateCurrentVote();
            }
            else
            {
                gb_currentVote.Controls.Clear();
                gb_currentVote.Controls.Add(status);
                status.Text = "No valid id!";
                lbl_activeID.Text = "None";
            }
        }

        private void updateCurrentVote()
        {
            refresher = new Thread(new ThreadStart(_updateCurrentVote));
            refresher.IsBackground = true;
            refresher.Start();
        }

        private void _updateCurrentVote()
        {
            try
            {
                if (working)
                {
                    status.Invoke((Action)delegate
                    {
                        status.Text = statusText;
                    });
                    return;
                }

                gb_currentVote.Invoke((Action)delegate
                {
                    gb_currentVote.Controls.Clear();
                    gb_currentVote.Controls.Add(status);
                });
                status.Invoke((Action)delegate
                {
                    status.Text = "Refreshing...";
                });

                currentTarget = new Uri(baseUrl + sessionID);
                HttpWebResponse response = connection.get(currentTarget);
            
                if (response == null)
                {
                    status.Invoke((Action)delegate
                    {
                        status.Text = "Got timeout!";
                    });
                }
                else if (response.StatusCode != HttpStatusCode.OK)
                {
                    status.Invoke((Action)delegate
                    {
                        status.Text = "Got error response! (" + response.StatusCode + ")";
                    });
                    response.Dispose();
                }
                else
                {
                    status.Invoke((Action)delegate
                    {
                        status.Text = "Got OK response!";
                    });
                    if (parseCurrentVote(response) == "not_running")
                    {
                        status.Invoke((Action)delegate
                        {
                            status.Text = "Poll not running yet... Retry in 10 seconds.";
                        });
                        Thread.Sleep(10000);
                        updateCurrentVote();
                    }
                    else
                    {
                        status.Invoke((Action)delegate
                        {
                            status.Text = statusText;
                        });
                        startGivingAnswers();
                    }
                }
            }
            catch (ThreadInterruptedException) { }
        }

        private String parseCurrentVote(HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            String html = reader.ReadToEnd();
            reader.Close();
            response.Dispose();
            
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);

            HtmlNode node = document.GetElementbyId("not_running");
            if (node != null)
            {
                return "not_running";
            }
            
            String pollTitle = document.DocumentNode.SelectSingleNode("//h2").InnerText;
            String question = document.DocumentNode.SelectSingleNode("//h3").InnerText;

            statusText = "Poll is running!\n" + pollTitle + "\n" + question;
            
            HtmlNode form = document.DocumentNode.SelectSingleNode("//form[@action='/vote']");

            //HtmlNodeCollection labels = form.SelectNodes(".//label");
            HtmlNodeCollection inputs = form.SelectNodes(".//input");
            
            List<String> possiblePostAdditions = new List<String>();
            Random rand = new Random();
            int randomInput;
            if (this.forceInput.Count > 0)
            {
                randomInput = this.forceInput[rand.Next(this.forceInput.Count)];
            }
            else
            {
                randomInput = rand.Next(int.MinValue, int.MaxValue);
            }

            StringBuilder sb = new StringBuilder();
            foreach (HtmlNode input in inputs)
            {
                String inputType = input.Attributes["type"].Value;
                String inputName = input.Attributes["name"].Value;
                String inputValue = "";
                if (input.Attributes["value"] != null)
                    inputValue = input.Attributes["value"].Value;
                if (inputValue == "")
                    inputValue = randomInput.ToString();

                inputName = HttpUtility.UrlEncode(inputName, Encoding.UTF8);
                inputValue = HttpUtility.UrlEncode(inputValue, Encoding.UTF8);

                if (inputType == "hidden" || inputType == "submit")
                    sb.Append(inputName + "=" + inputValue + "&");
                else //if (inputType == "radio" || inputType == "checkbox")
                    possiblePostAdditions.Add(inputName + "=" + inputValue);
            }
            String basePost = sb.ToString();

            String postAddition = possiblePostAdditions[rand.Next(possiblePostAdditions.Count)];
            if (this.forceInput.Count > 0)
            {
                int index = this.forceInput[rand.Next(this.forceInput.Count)];
                if (index >= 0 && index < possiblePostAdditions.Count)
                    postAddition = possiblePostAdditions[index];
            }

            return basePost + postAddition;
        }

        private void startGivingAnswers()
        {
            stopGivingAnswers();

            this.votesGiven = 0;
            this.working = true;

            workers = new List<Thread>();
            for (int i = 0; i < this.threadCount; i++)
            {
                Thread t = new Thread(new ThreadStart(answerThread));
                t.IsBackground = true;
                t.Start();
                workers.Add(t);
            }
        }

        private void stopGivingAnswers()
        {
            this.working = false;

            foreach (Thread t in workers)
                if (t.IsAlive)
                    t.Join();
        }

        private void answerThread()
        {
            try
            {
                while (this.working)
                {
                    Helper connection = new Helper();
                    connection.acceptCookies = true;
                    connection.spoofIPAddress = IPSpoofing.GenerateIPAddress();
                    connection.userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:62.0) Gecko/20100101 Firefox/62.0";

                    HttpWebResponse response = connection.get(currentTarget);

                    if (response == null || response.StatusCode != HttpStatusCode.OK)
                    {
                        if (response != null)
                            response.Dispose();
                        this.working = false;
                        status.Invoke((Action)delegate
                        {
                            status.Text = "Got timeout or error!";
                        });
                        return;
                    }

                    String post = parseCurrentVote(response);

                    if (post == "not_running")
                    {
                        this.working = false;
                        status.Invoke((Action)delegate
                        {
                            status.Text = "Poll no longer running!";
                        });
                        return;
                    }

                    Logger.debug("IP-Address: " + connection.spoofIPAddress, "Cookies: " + connection.cookies.Count, "Will post: " + post);

                    Thread.Sleep(waitMillis);
                    HttpWebResponse resp = connection.post(voteTarget, post);
                    resp.Dispose();

                    this.votesGiven++;
                    Thread.Sleep(this.waitMillis);
                }
            }
            catch (Exception e)
            {
                Program.LogException(e);
            }
        }

        private void cb_logging_CheckedChanged(object sender, EventArgs e)
        {
            Logger.logLevel = cb_logging.Checked ? Logger.LOGLEVEL_DEBUG : Logger.LOGLEVEL_ERROR;
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            if (refresher != null && refresher.IsAlive)
                refresher.Abort();
            stopGivingAnswers();

            gb_currentVote.Controls.Clear();
            gb_currentVote.Controls.Add(status);
            status.Text = "Stopped!";
        }

        private void timer_votesUpdate_Tick(object sender, EventArgs e)
        {
            lbl_votes.Text = this.votesGiven.ToString();
        }
    }
}
