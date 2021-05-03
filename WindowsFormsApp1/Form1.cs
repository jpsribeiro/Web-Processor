using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            WebProcessor t = new WebProcessor();

            t.GetGeneratedHTML("http://cnes.datasus.gov.br/pages/profissionais/consulta.jsp?search=13165139717");

            string CNS = t.GeneratedSource.Substring(21495, 15);

            string[] LinhaNome = t.GeneratedSource.Substring(21677, 50).Split('\n');

            string ProfNome = LinhaNome[0];
        }
    }

    public class WebProcessor
    {
        public string GeneratedSource { get; set; }
        public string URL { get; set; }

        public string GetGeneratedHTML(string url)
        {
            URL = url;

            Thread t = new Thread(new ThreadStart(WebBrowserThread));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            return GeneratedSource;
        }

        private void WebBrowserThread()
        {
            WebBrowser wb = new WebBrowser();
            wb.ScriptErrorsSuppressed = true;
            wb.Navigate(URL);

            wb.DocumentCompleted +=
                new WebBrowserDocumentCompletedEventHandler(
                    wb_DocumentCompleted);

            while (wb.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            //Added this line, because the final HTML takes a while to show up
            GeneratedSource = wb.Document.Body.InnerHtml;

            wb.Dispose();
        }

        private void wb_DocumentCompleted(object sender,
            WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser wb = (WebBrowser)sender;
            GeneratedSource = wb. Document.Body.InnerHtml;
        }
    }
}
