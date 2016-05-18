using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OPR.lb2.Enums;
using OPR.SSGA2;
using OPR.SSGA2.Extension;
using OPR.SSGA2.Interfaces;

namespace Lab2.UI
{
    public static class GridHandler
    {
        public static void UpdateGrid<TV, TG>(
            DataGridView dataGridView1,
            IList<Generation<TV, TG>> generations,
            Action<Entity<TV, TG>> createRow)
            where TV : IValueService, new()
        where TG : IGenom, new()
        {
            dataGridView1.Rows.Clear();
            foreach (var binaryGeneration in generations)
            {
                Drawline(dataGridView1);
                for (var i = 0; i < binaryGeneration.Parents.Count; ++i)
                {
                    var parent = binaryGeneration.Parents[i];
                    createRow(parent);
                }

                if (binaryGeneration.Children != null && binaryGeneration.Children.Any())
                {
                    var groups = binaryGeneration.Children.GroupBy(x => x.Type);
                    foreach (var group in groups)
                    {
                        DrawLineChildrenSeparator(GetGroupMsg(group.Key,
                            binaryGeneration.GetChilrensCrossingPoint().ToString()), dataGridView1);
                        foreach (var item in group)
                        {
                            createRow(item);
                        }
                    }
                }
            }
        }

        private static void Drawline(DataGridView dataGridView1)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                var row = ColorizedRow(Color.Black);
                dataGridView1.Rows.Add(row);
            }
        }

        private static void DrawLineChildrenSeparator(string msg, DataGridView dataGridView1)
        {
            var color = Color.Yellow;
            var row = ColorizedRow(color);
            row.Cells.AddRange(Cells(Color.Yellow, dataGridView1).ToArray());
            row.Cells[3].Value = "Потомки";
            row.Cells[4].Value = msg;
            dataGridView1.Rows.Add(row);
        }

        public static IList<DataGridViewCell> Cells(Color color, DataGridView dataGridView1)
        {
            var cells = new List<DataGridViewCell>();
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                var cell = new DataGridViewTextBoxCell();
                cell.Style.BackColor = color;
                cells.Add(cell);
            }

            return cells;
        }

        public static DataGridViewRow GetColorizedRow<TV, TG>(Entity<TV, TG> entity)
            where TV : IValueService, new()
        where TG : IGenom, new()
        {
            switch (entity.Function)
            {
                case EntityFunction.BestParent:
                    return ColorizedRow(Color.Green);
                case EntityFunction.WorstParent:
                    return ColorizedRow(Color.Red);
                case EntityFunction.BestChild:
                    return ColorizedRow(Color.Green);
                case EntityFunction.NotValid:
                    return ColorizedRow(Color.BlueViolet);
                case EntityFunction.None:
                default:
                    return ColorizedRow(Color.White);
            }
        }

        private static DataGridViewRow ColorizedRow(Color color)
        {
            var datagridRow = new DataGridViewRow();
            datagridRow.DefaultCellStyle.BackColor = color;
            return datagridRow;
        }

        private static string GetGroupMsg(EntityType type, string crossPoit)
        {
            if (type == EntityType.Child)
            {
                return string.Format("{0} - {1}", type.ToString(), crossPoit);
            }

            return type.ToString();
        }

    }
}
