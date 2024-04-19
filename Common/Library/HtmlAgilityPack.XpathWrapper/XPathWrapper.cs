namespace HtmlAgilityPack.XpathWrapper
{
    using HtmlAgilityPack;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;

    public class XPathWrapper
    {
        private HtmlDocument _document;

        public XPathWrapper(string html)
        {
            _document = new HtmlDocument();
            _document.LoadHtml(html);
        }

        // Asynchronous HTML loader
        public async Task LoadHtmlAsync()
        {
            await Task.Run(() => _document.LoadHtml(_document.Text));
        }

        // Select Single Node
        public HtmlNode SelectSingleNode(string xpath) => _document.DocumentNode.SelectSingleNode(xpath);

        // Select Multiple Nodes
        public List<HtmlNode> SelectNodes(string xpath) => _document.DocumentNode.SelectNodes(xpath)?.ToList() ?? new List<HtmlNode>();

        // Select Attribute Value
        //public string SelectAttributeValue(string xpath, string attribute) => SelectSingleNode(xpath)?.GetAttributeValue(attribute, default);

        // Select Nodes by Class Name
        public List<HtmlNode> SelectByClass(string className) => SelectNodes($"//*[contains(@class, '{className}')]");

        // Select Node by ID
        public HtmlNode SelectById(string id) => SelectSingleNode($"//*[@id='{id}']");

        // Select Text of a Node
        public string SelectText(string xpath) => SelectSingleNode(xpath)?.InnerText.Trim();

        // Select Nodes that Contain Text
        public List<HtmlNode> SelectContainingText(string text) => SelectNodes($"//*[contains(text(), '{text}')]");

        // Select Sibling Nodes
        public List<HtmlNode> SelectSiblings(string xpath) => SelectSingleNode(xpath)?.ParentNode.ChildNodes.Where(n => n != SelectSingleNode(xpath)).ToList();

        // Select Parent Node
        public HtmlNode SelectParent(string xpath) => SelectSingleNode(xpath)?.ParentNode;

        // Select Child Nodes
        public List<HtmlNode> SelectChildren(string xpath) => SelectSingleNode(xpath)?.ChildNodes.ToList();

        // Select First Node
        public HtmlNode SelectFirst(string xpath) => SelectNodes(xpath).FirstOrDefault();

        // Select Last Node
        public HtmlNode SelectLast(string xpath) => SelectNodes(xpath).LastOrDefault();

        // Select Nth of Type
        public HtmlNode SelectNthOfType(string xpath, int n) => SelectNodes(xpath).ElementAtOrDefault(n - 1);

        // Usage of various operators like >, +, ~, etc.
        public HtmlNode SelectDirectChild(string parentXPath, string childType) => SelectSingleNode($"{parentXPath}>{childType}");

        public HtmlNode SelectAdjacentSibling(string xpath, string siblingType) => SelectSingleNode($"{xpath}+{siblingType}");

        public List<HtmlNode> SelectGeneralSiblings(string xpath, string siblingType) => SelectNodes($"{xpath}~{siblingType}");
    }

}
