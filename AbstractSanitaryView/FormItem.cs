using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AbstractSanitaryService.ViewModels;
using System.Threading.Tasks;
using AbstractSanitaryService.BindingModels;

namespace AbstractSanitaryView
{
    public partial class FormItem : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        private List<ItemPartViewModel> itemParts;

        public FormItem()
        {
            InitializeComponent();
        }

        private void FormItem_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var item = Task.Run(() => APIClient.GetRequestData<ItemViewModel>("api/Item/Get/" + id.Value)).Result;
                    textBoxName.Text = item.ItemName;
                    textBoxPrice.Text = item.Price.ToString();
                    itemParts = item.ItemParts;
                    LoadData();
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                itemParts = new List<ItemPartViewModel>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (itemParts != null)
                {
                    dataGridView.DataSource = null;
                    dataGridView.DataSource = itemParts;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormItemPart();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                    {
                        form.Model.ItemId = id.Value;
                    }
                    itemParts.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = new FormItemPart();
                form.Model = itemParts[dataGridView.SelectedRows[0].Cells[0].RowIndex];
                if (form.ShowDialog() == DialogResult.OK)
                {
                    itemParts[dataGridView.SelectedRows[0].Cells[0].RowIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        itemParts.RemoveAt(dataGridView.SelectedRows[0].Cells[0].RowIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Введите название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Введите цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (itemParts == null || itemParts.Count == 0)
            {
                MessageBox.Show("Выберите компоненты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<ItemPartBindingModel> itemPartBM = new List<ItemPartBindingModel>();
            for (int i = 0; i < itemParts.Count; ++i)
            {
                itemPartBM.Add(new ItemPartBindingModel
                {
                    Id = itemParts[i].Id,
                    ItemId = itemParts[i].ItemId,
                    PartId = itemParts[i].PartId,
                    Count = itemParts[i].Count
                });
            }
            string name = textBoxName.Text;
            int price = Convert.ToInt32(textBoxPrice.Text);
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Item/UpdElement", new ItemBindingModel
                {
                    Id = id.Value,
                    ItemName = name,
                    Price = price,
                    ItemParts = itemPartBM
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Item/AddElement", new ItemBindingModel
                {
                    ItemName = name,
                    Price = price,
                    ItemParts = itemPartBM
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
