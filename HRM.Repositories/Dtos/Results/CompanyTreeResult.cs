using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Results
{

    public class TreeNode
    {
        public string Key { get; set; } = "";
        public string Label { get; set; } = "";
        public string Data { get; set; } = "";
        public string Icon { get; set; } = "";
        public List<TreeNode> Children { get; set; } = new List<TreeNode>();

    }

    public class RowNode
    {
        public int ParentId { get; set; } = 0;
        public int CurrentId { get; set; } = 0;
        public string Name { get; set; } = "";

    }




}
