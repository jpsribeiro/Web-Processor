using System.Threading;
using System.Windows.Forms;

public class Form1
{
    private string GeneratedSource { get; set; }
    private string URL { get; set; }

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
        wb.Navigate(URL);

        wb.DocumentCompleted +=
            new WebBrowserDocumentCompletedEventHandler(
                wb_DocumentCompleted);

        while (wb.ReadyState != WebBrowserReadyState.Complete)
            Application.DoEvents();

        //Added this line, because the final HTML takes a while to show up
        GeneratedSource = wb.Document.Body.InnerHtml;

        wb.Dispose();
    }

    private void wb_DocumentCompleted(object sender,
        WebBrowserDocumentCompletedEventArgs e)
    {
        WebBrowser wb = (WebBrowser)sender;
        GeneratedSource = wb.Document.Body.InnerHtml;
    }
}