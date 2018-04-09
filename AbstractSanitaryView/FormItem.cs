using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using AbstractSanitaryService.BindingModels;

namespace AbstractSanitaryView
{
    public partial class FormItem : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IItemService service;

        private int? id;

        private List<ItemPartViewModel> itemParts;

        public FormItem(IItemService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormItem_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    ItemViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.ItemName;
                        textBoxPrice.Text = view.Price.ToString();
                        itemParts = view.ItemParts;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
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
            var form = Container.Resolve<FormItemPart>();
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
                var form = Container.Resolve<FormItemPart>();
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
            try
            {
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
                if (id.HasValue)
                {
                    service.UpdElement(new ItemBindingModel
                    {
                        Id = id.Value,
                        ItemName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        ItemParts = itemPartBM
                    });
                }
                else
                {
                    service.AddElement(new ItemBindingModel
                    {
                        ItemName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        ItemParts = itemPartBM
                    });
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
