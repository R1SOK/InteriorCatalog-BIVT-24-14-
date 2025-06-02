using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Model.Core;
using Model.Data;
using System.IO;
using System.Drawing;
using System.Data;

namespace InteriorCatalog.UI
{
    public class MainForm : Form
    {
        private ComboBox catalogSelector;
        private Button showButton;
        private DataGridView furnitureGrid;
        private ComboBox typeFilter;
        private Button groupButton;
        private Button sortByPriceButton;
        private Button sortByNameButton;
        private Button sortByArticleButton;
        private ComboBox formatSelector;
        private Label infoLabel;
        private bool sortByPriceAsc = true;
        private bool sortByNameAsc = true;
        private bool sortByArticleAsc = true;

        private FurnitureCatalog currentCatalog = new();

        public MainForm()
        {
            Text = "Interior Catalog";
            Width = 1000;
            Height = 702;
            this.BackColor = Color.White;
            


            catalogSelector = new ComboBox
            {
                Left = 20,
                Top = 20,
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                ItemHeight = 22,
                Height = 16,
            };
            catalogSelector.Items.AddRange(new[] { "Summer", "Winter", "Autumn", "Mega Catalog" });


            catalogSelector.DrawItem += (s, e) =>
            {
                e.DrawBackground();

                string text;
                if (e.Index < 0)
                {
                    text = "Каталог";
                }
                else
                {
                    text = catalogSelector.Items[e.Index].ToString();
                }
                using var brush = new SolidBrush(catalogSelector.ForeColor);
                e.Graphics.DrawString(text, catalogSelector.Font, brush, e.Bounds);
                e.DrawFocusRectangle();
                catalogSelector.SelectionChangeCommitted += (s, e) =>
                {
                    this.ActiveControl = null;
                };
            };
            showButton = new Button
            {
                Text = "Показать каталог",
                Left = 240,
                Top = 18,
                Width = 140,
                Height = 32,
                Font = new Font("Segoe UI", 10F)
            };
            showButton.Click += ShowButton_Click;

            typeFilter = new ComboBox
            {
                Left = 20,
                Top = 62,
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                Height = 30
            };
            typeFilter.Items.AddRange(new[] { "Все", "Chair", "Table", "Sofa", "Bed", "Stool", "Armchair" });
            typeFilter.SelectedIndexChanged += TypeFilter_SelectedIndexChanged;

            sortByArticleButton = new Button
            {
                Text = "Артикул ↑↓",
                Left = 240,
                Top = 60,
                Width = 100,
                Height = 30,
                Font = new Font("Segoe UI", 10F)
            };
            sortByArticleButton.Click += (s, e) => Sort((a, b) => a.Article.CompareTo(b.Article));

            sortByNameButton = new Button
            {
                Text = "Название ↑↓",
                Left = 350,
                Top = 60,
                Width = 100,
                Height = 30,
                Font = new Font("Segoe UI", 10F)
            };
            sortByNameButton.Click += (s, e) => Sort((a, b) => a.ModelName.CompareTo(b.ModelName));

            sortByPriceButton = new Button
            {
                Text = "Цена ↑↓",
                Left = 460,
                Top = 60,
                Width = 100,
                Height = 30,
                Font = new Font("Segoe UI", 10F)
            };
            sortByPriceButton.Click += (s, e) =>
            {
            Sort((a, b) => sortByPriceAsc ? a.Price.CompareTo(b.Price) : b.Price.CompareTo(a.Price));
            sortByPriceAsc = !sortByPriceAsc;
            sortByPriceButton.Text = sortByPriceAsc ? "Цена ↑" : "Цена ↓";
            };
            sortByNameButton.Click += (s, e) =>
            {
                Sort((a, b) => sortByNameAsc ? a.ModelName.CompareTo(b.ModelName) : b.ModelName.CompareTo(a.ModelName));
                sortByNameAsc = !sortByNameAsc;
                sortByNameButton.Text = sortByNameAsc ? "Название ↑" : "Название ↓";
            };
            
            sortByArticleButton.Click += (s, e) =>
            {
                Sort((a, b) => sortByArticleAsc ? a.Article.CompareTo(b.Article) : b.Article.CompareTo(a.Article));
                sortByArticleAsc = !sortByArticleAsc;
                sortByArticleButton.Text = sortByArticleAsc ? "Артикул ↑" : "Артикул ↓";
            };

            var sumButton = new Button
            {
                Text = "Сумма цен",
                Left = 820,
                Top = 60,
                Width = 120,
                Height = 30,
                Font = new Font("Segoe UI", 10F)
            };
            sumButton.Click += SumButton_Click;
            Controls.Add(sumButton);




            groupButton = new Button
            {
                Text = "Сгруппировать",
                Left = 570,
                Top = 60,
                Width = 120,
                Height = 30,
                Font = new Font("Segoe UI", 10F)
            };
            groupButton.Click += GroupButton_Click;

            formatSelector = new ComboBox
            {
                Left = 700,
                Top = 62,
                Width = 100,
                Height = 34,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)

            };
            formatSelector.Items.AddRange(new[] { "JSON", "XML" });
            formatSelector.SelectedIndex = 0;

            formatSelector.SelectedIndexChanged += (s, e) =>
            {
                string newFormat = formatSelector.SelectedItem?.ToString();
                string oldFormat = newFormat == "XML" ? "json" : "xml";

                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), $"catalog-*.{oldFormat}");
                foreach (var oldFile in files)
                {
                    try
                    {
                        AbstractSerializer<FurnitureCatalog> oldSerializer = oldFormat == "json"
                            ? new JsonSerializer<FurnitureCatalog>()
                            : new XmlSerializerAdapter<FurnitureCatalog>();

                        AbstractSerializer<FurnitureCatalog> newSerializer = newFormat == "XML"
                            ? new XmlSerializerAdapter<FurnitureCatalog>()
                            : new JsonSerializer<FurnitureCatalog>();

                        var data = oldSerializer.Deserialize(oldFile);

                        string newFile = oldFile.Replace($".{oldFormat}", $".{newFormat.ToLower()}");
                        newSerializer.Serialize(newFile, data);
                        Console.WriteLine($"> Converted: {oldFile} → {newFile}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка конвертации файла {oldFile}: {ex.Message}");
                    }
                }
            };


            furnitureGrid = new DataGridView
            {
                Left = 20,
                Top = 100,
                Width = 940,
                Height = 500,

                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    WrapMode = DataGridViewTriState.True
                }
            };
            furnitureGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            furnitureGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //адаптив
            furnitureGrid.BackgroundColor = Color.White;

            furnitureGrid.BackgroundColor = Color.LightGray;//цвета
            furnitureGrid.DefaultCellStyle.BackColor = Color.LightGray;//цвета

            furnitureGrid.CellDoubleClick += FurnitureGrid_CellDoubleClick;

            infoLabel = new Label { Left = 20, Top = 620, Width = 940 };

            Controls.AddRange(new Control[]
            {
                catalogSelector, showButton, typeFilter,
                sortByArticleButton, sortByNameButton, sortByPriceButton,
                groupButton, formatSelector, furnitureGrid, infoLabel
            });
        }
        private void SumButton_Click(object? sender, EventArgs e)
        {
            if (furnitureGrid.SelectedRows.Count != 2)
            {
                MessageBox.Show("Выберите ровно 2 строки для сложения.");
                return;
            }

            var selectedIds = furnitureGrid.SelectedRows
                .Cast<DataGridViewRow>()
                .Select(row => row.Cells["Код"].Value?.ToString())
                .ToList();

            var items = currentCatalog.Items
                .Where(f => selectedIds.Contains(f.Id))
                .ToList();

            if (items.Count == 2)
            {
                decimal total = items[0] + items[1];
                MessageBox.Show($"Сумма цен: {total:C}");
            }
            else
            {
                MessageBox.Show("Ошибка: не удалось найти выбранные объекты.");
            }
        }



        private void ShowButton_Click(object? sender, EventArgs e)
        {
            
            if (catalogSelector.SelectedItem == null)
            {
                MessageBox.Show("Выберите каталог");
                return;
            }

            string name = catalogSelector.SelectedItem.ToString();
            string format = formatSelector.SelectedItem?.ToString()?.ToLower() ?? "json";

            string fileName = name.ToLower() switch
            {
                "summer" => $"catalog-summer.{format}",
                "winter" => $"catalog-winter.{format}",
                "autumn" => $"catalog-autumn.{format}",
                "mega catalog" => $"catalog-mega.{format}",
                _ => $"catalog.{format}"
            };
            
            if (!File.Exists(fileName))
            {
                MessageBox.Show($"Файл {fileName} не существует!");
                return;
            }

            var contents = File.ReadAllText(fileName);
            if (string.IsNullOrWhiteSpace(contents))
            {
                MessageBox.Show($"Файл {fileName} пустой или повреждён!");
                return;
            }

            

            AbstractSerializer<FurnitureCatalog> serializer =
                formatSelector.SelectedItem?.ToString() == "XML"
                ? new XmlSerializerAdapter<FurnitureCatalog>()
                : new JsonSerializer<FurnitureCatalog>();

            if (System.IO.File.Exists(fileName))
            {
                try
                {
                    var loaded = serializer.Deserialize(fileName);
                    currentCatalog = loaded;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки каталога: " + ex.Message);
                    return;
                }
            }
            else
            {
                MessageBox.Show($"Файл {fileName} не найден.");
                return;
            }

            DisplayFiltered();
        }

        private void LoadCatalogFromXml(string path)
        {

            try
            {
                var serializer = new XmlSerializerAdapter<FurnitureCatalog>();
                FurnitureCatalog catalog = serializer.Deserialize(path);

                MessageBox.Show($"Загружен каталог: {catalog.Name} ({catalog.Season})\nПредметов: {catalog.Items.Count}");


            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке XML: " + ex.Message);
            }
        }

        private void DisplayFiltered()
        {
            if (currentCatalog == null) return;

            IEnumerable<Furniture> items = currentCatalog.Items;

            if (typeFilter.SelectedItem?.ToString() != "Все" && !string.IsNullOrWhiteSpace(typeFilter.SelectedItem?.ToString()))
            {
                string selectedType = typeFilter.SelectedItem.ToString()!;
                items = items.Where(x => x.GetType().Name == selectedType);
            }

            var table = new DataTable();
            table.Columns.Add("Фото", typeof(Image));
            table.Columns.Add("Код", typeof(string));
            table.Columns.Add("Название", typeof(string));
            table.Columns.Add("Бренд", typeof(string));
            table.Columns.Add("Артикул", typeof(string));
            table.Columns.Add("Цена", typeof(string));
            table.Columns.Add("Описание", typeof(string));

            foreach (var item in items)
            {
                Image img;
                try
                {
                    img = File.Exists(item.ImagePath) ? Image.FromFile(item.ImagePath) : null;
                }
                catch
                {
                    img = null;
                }

                string description = item switch
                {
                    Armchair a => $"Спинка: {(a.IsReclining ? "да" : "нет")}",
                    Stool s => $"Мягкий: {(s.IsSoft ? "да" : "нет")}",
                    Chair c => $"Спинка: {(c.HasBackSupport ? "да" : "нет")}, Ножек: {c.Legs}",
                    Table t => $"Площадь: {t.Area}, Складной: {(t.IsFolding ? "да" : "нет")}",
                    Sofa s => $"Мест: {s.Seats}, Хранение: {(s.HasStorage ? "да" : "нет")}",
                    Bed b => $"Размер: {b.Size}, Изголовье: {(b.HasHeadboard ? "да" : "нет")}" +
         (b.IsBunk ? ", двухъярусная" : ""),
                    _ => "—"
                };

                table.Rows.Add(img, item.Id, item.ModelName, item.Brand, item.Article, $"{item.Price:C}", description);
            }

            furnitureGrid.Columns.Clear();
            furnitureGrid.DataSource = table;
            if (furnitureGrid.Columns["Код"] is DataGridViewColumn idCol)
            {
                idCol.FillWeight = 50;
            }

            if (furnitureGrid.Columns["Описание"] is DataGridViewColumn descCol)
            {
                descCol.FillWeight = 150;
            }

            furnitureGrid.RowTemplate.Height = 3120;
            foreach (DataGridViewRow row in furnitureGrid.Rows)
            {
                row.Height = 120;
            }

            if (furnitureGrid.Columns[0] is DataGridViewImageColumn imgCol)
            {
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                imgCol.Width = 120;
            }

            infoLabel.Text = $"Элементов: {items.Count()}";
        }


        private void Sort(Comparison<Furniture> comparison)
        {
            if (currentCatalog == null) return;
            currentCatalog.Items.Sort(comparison);
            DisplayFiltered();
        }

        private void GroupButton_Click(object? sender, EventArgs e)
        {
            currentCatalog?.PrioritySort();
            DisplayFiltered();
        }

        private void FurnitureGrid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = furnitureGrid.Rows[e.RowIndex];
            string? modelName = row.Cells["Название"].Value?.ToString();
            string? article = row.Cells["Артикул"].Value?.ToString();

            var furniture = currentCatalog.Items
                .FirstOrDefault(f => f.ModelName == modelName && f.Article == article);

            if (furniture == null)
            {
                MessageBox.Show("Элемент не найден.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(furniture.ImagePath) && File.Exists(furniture.ImagePath))
            {
                var imageForm = new ImageForm(furniture.ImagePath);
                imageForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Изображение не найдено для: " + furniture.ModelName);
            }
        }
        private void TypeFilter_SelectedIndexChanged(object? sender, EventArgs e)
        {
            DisplayFiltered();
        }
        private void loadXmlButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "XML файлы|*.xml"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                LoadCatalogFromXml(ofd.FileName);
            }
        }

    }
}
