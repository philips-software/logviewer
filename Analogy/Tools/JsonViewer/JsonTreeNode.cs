using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;

namespace Analogy.Tools.JsonViewer
{
    class JsonTreeNode : TreeNode
    {
        public JsonNodeType NodeType { get; }

        public string TextWhenSelected { get; }

        public bool IsExpandable => NodeType == JsonNodeType.Object || NodeType == JsonNodeType.Array;

        public JsonTreeNode(JsonNodeType nodeType, string text, string? textWhenSelected = null)
        {
            NodeType = nodeType;
            Text = text;
            TextWhenSelected = textWhenSelected ?? text;
        }
    }
}