using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using Istra.Entities;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PawnHunter.Numerals;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Xceed.Words.NET;
using Font = Xceed.Words.NET.Font;

namespace Istra.Documents
{
    enum Months { Января = 1, Февраля, Марта, Апреля, Мая, Июня, Июля, Августа, Сентября, Октября, Ноября, Декабря };
    public class Report
    {
        

        public void ExportContract(PrintContract data, string typeFill, string typeCorp, bool agreement, bool shed)
        {
            double sum = 0.0;
            //проверка пути и возможности создания файла
            string path = Directory.GetCurrentDirectory();

            Regex pattern = new Regex("[\\/|<>?*:\"]");
            var newGroupName = pattern.Replace(data.Group.Name, "_");

            string pathCopy = path + "\\Договора\\" + data.Year.Name + "\\" + newGroupName + "\\";

            //создание копии договора
            if (!Directory.Exists(pathCopy))
            {
                Directory.CreateDirectory(pathCopy);
            }

            var newFileName = pathCopy + $"Договор-{data.Student.Lastname}-{DateTime.Now.ToShortDateString()}.docx";
            DialogResult dr;
            if (File.Exists(newFileName))
                dr = MessageBox.Show("Договор на учащегося уже создан! Открыть его? Нажмите \"Да\", чтобы открыть имеющийся договор, или \"Нет\" для создания нового договора.", "Внимание", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            else
                dr = DialogResult.None;
            if (dr == DialogResult.No || dr == DialogResult.None)
            {

                if (typeFill == "nothing")
                {
                    if (typeCorp == "CHOU")
                    {
                        File.Copy(path + "\\Шаблоны\\Договор-уч-год-пустой-шаблон.docx", newFileName, true);
                    }
                    else if (typeCorp == "camp")
                    {
                        File.Copy(path + "\\Шаблоны\\Договор-лагерь-пустой-шаблон.docx", newFileName, true);
                    }
                    else
                    {
                        File.Copy(path + "\\Шаблоны\\Договор-ИП-пустой-шаблон.docx", newFileName, true);
                    }
                }
                else
                {
                    if (typeCorp == "CHOU")
                    {
                        File.Copy(path + "\\Шаблоны\\Договор-уч-год-род-шаблон.docx", newFileName, true);
                    }
                    else if (typeCorp == "camp")
                    {
                        File.Copy(path + "\\Шаблоны\\Договор-лагерь-шаблон.docx", newFileName, true);
                    }
                    else
                    {
                        File.Copy(path + "\\Шаблоны\\Договор-ИП-род-шаблон.docx", newFileName, true);
                    }
                }

                DocX letter = DocX.Load(pathCopy + $"Договор-{data.Student.Lastname}-{DateTime.Now.ToShortDateString()}.docx");
                letter.ReplaceText("[Год]", DateTime.Now.Year.ToString());
                
                if (typeFill != "nothing")
                {
                    letter.ReplaceText("[Число]", DateTime.Now.Day.ToString());
                    int m = DateTime.Now.Month;
                    Months month = (Months)m;
                    letter.ReplaceText("[Месяц]", month.ToString());

                    //заполнение ФИО заказчика
                    if (data.Student.LastnameParent != null && data.Student.FirstnameParent != null)
                    {
                        if (!String.IsNullOrEmpty(data.Student.MiddlenameParent))
                        {
                            letter.ReplaceText("[ФИОЗаказчика]", data.Student.LastnameParent + " " + data.Student.FirstnameParent + " " + data.Student.MiddlenameParent);
                        }
                        else
                        {
                            letter.ReplaceText("[ФИОЗаказчика]", data.Student.LastnameParent + " " + data.Student.FirstnameParent);
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(data.Student.Middlename))
                        {
                            letter.ReplaceText("[ФИОЗаказчика]", data.Student.Lastname + " " + data.Student.Firstname + " " + data.Student.Middlename);
                        }
                        else
                        {
                            letter.ReplaceText("[ФИОЗаказчика]", data.Student.Lastname + " " + data.Student.Firstname);
                        }
                    }

                    if (!String.IsNullOrEmpty(data.Student.Middlename))
                    {
                        letter.ReplaceText("[ФИОРебенка]", data.Student.Lastname + " " + data.Student.Firstname + " " + data.Student.Middlename);
                    }
                    else
                    {
                        letter.ReplaceText("[ФИОРебенка]", data.Student.Lastname + " " + data.Student.Firstname);
                    }

                    letter.ReplaceText("[Направление]", data.Direction.Name);
                    //letter.ReplaceText("[УровеньКурс]", data.Course.Name);
                    letter.ReplaceText("[КоличествоЧасов]", data.Group.DurationCourse.ToString());
                    letter.ReplaceText("[ДокументОбОбразовании]", data.Document.Name);
                    //стоимость
                    sum = data.Schedules.Select(a => a.Val).Sum();
                    letter.ReplaceText("[СтоимостьОбученияЧисло]", sum.ToString());
                    //прописью
                    NumeralsFormatter formatter = new NumeralsFormatter();
                    formatter.CultureInfo = new CultureInfo("ru-Ru");
                    string format = "{0:T;M}";
                    var valueCourse = String.Format(formatter, format, (int)sum, 0);
                    letter.ReplaceText("[СтоимостьОбученияПрописью]", valueCourse);
                }

                if (typeFill == "withoutRecvisits")
                {
                    letter.ReplaceText("[ФамилияРодителя]", "");
                    letter.ReplaceText("[ИмяРодителя]", "");
                    letter.ReplaceText("[ОтчествоРодителя]", "");
                    letter.ReplaceText("[ТелефонРодителей]", "");

                    letter.ReplaceText("[Город]", "");
                    letter.ReplaceText("[Адрес]", "");
                    letter.ReplaceText("[СерияНомер]", "");
                    letter.ReplaceText("[КемВыдан]", "");
                    letter.ReplaceText("[ДатаВыдачи]", "");                    
                    letter.ReplaceText("[ФамилияРебенка]", "");
                    letter.ReplaceText("[ИмяРебенка]", "");
                    letter.ReplaceText("[ОтчествоРебенка]", "");
                    letter.ReplaceText("[ДатаРождения]", "");
                    letter.ReplaceText("[ТелефонРебенка]", "");
                    letter.ReplaceText("[ДопТелефонРебенка]", "");
                    letter.ReplaceText("[ГородРебенка]", "");
                    letter.ReplaceText("[АдресРебенка]", "");
                }

                if (typeFill == "fullFill")
                {
                    if (data.Student.LastnameParent != null && data.Student.FirstnameParent != null)
                    {
                        letter.ReplaceText("[ФамилияРодителя]", data.Student.LastnameParent);
                        letter.ReplaceText("[ИмяРодителя]", data.Student.FirstnameParent);
                        letter.ReplaceText("[ОтчествоРодителя]", data.Student.MiddlenameParent ?? "");
                        letter.ReplaceText("[ТелефонРодителей]", String.Format("{0:(###) ###-##-##}", data.Student.ParentsPhone) ?? "");
                        
                        letter.ReplaceText("[ФамилияРебенка]", data.Student.Lastname ?? "");
                        letter.ReplaceText("[ИмяРебенка]", data.Student.Firstname ?? "");
                        letter.ReplaceText("[ОтчествоРебенка]", data.Student.Middlename ?? "");
                        letter.ReplaceText("[ДатаРождения]", (data.Student.DateOfBirth.ToShortDateString() != "") ? data.Student.DateOfBirth.ToShortDateString() : "");
                        letter.ReplaceText("[ТелефонРебенка]", String.Format("{0:(###) ###-##-##}", data.Student.StudentPhone) ?? "");
                        letter.ReplaceText("[ДопТелефонРебенка]", String.Format("{0:(###) ###-##-##}", data.Student.StudentPhone2) ?? "");
                        letter.ReplaceText("[ГородРебенка]", data.City.Name ?? "");
                        letter.ReplaceText("[АдресРебенка]", (data.Student.Street.Name != null) ? data.Student.Street.Name + ",  д." + data.Student.House + ",  кв." + data.Student.Float ?? "" : "");
                    }
                    else
                    {
                        letter.ReplaceText("[ФамилияРодителя]", data.Student.Lastname ?? "");
                        letter.ReplaceText("[ИмяРодителя]", data.Student.Firstname ?? "");
                        letter.ReplaceText("[ОтчествоРодителя]", data.Student.Middlename ?? "");
                        letter.ReplaceText("[ТелефонРодителей]", String.Format("{0:(###) ###-##-##}", data.Student.StudentPhone) ?? "");
                        
                        letter.ReplaceText("[ФамилияРебенка]", "");
                        letter.ReplaceText("[ИмяРебенка]", "");
                        letter.ReplaceText("[ОтчествоРебенка]", "");
                        letter.ReplaceText("[ДатаРождения]", "");
                        letter.ReplaceText("[ТелефонРебенка]", "");
                        letter.ReplaceText("[ДопТелефонРебенка]", "");
                        letter.ReplaceText("[ГородРебенка]", "");
                        letter.ReplaceText("[АдресРебенка]", "");
                    }

                    letter.ReplaceText("[Город]", data.City.Name ?? "");
                    letter.ReplaceText("[Адрес]", (data.Student.Street.Name != null) ? data.Student.Street.Name + ",  д." + data.Student.House + ",  кв." + data.Student.Float ?? "" : "");
                    letter.ReplaceText("[СерияНомер]", data.Student.PassportNumber ?? "");
                    letter.ReplaceText("[КемВыдан]", data.Student.PassportIssuedBy ?? "");
                    letter.ReplaceText("[ДатаВыдачи]", (data.Student.PassportDate.Value != null) ? data.Student.PassportDate.Value.ToShortDateString() : "");                                        
                }

                if (shed && typeCorp == "CHOU")
                {
                    CreateShedulePays(data, letter, sum);
                }

                letter.Save();
            }
            if (dr != DialogResult.Cancel)
                Process.Start(newFileName);

            if (agreement)
            {
                Process.Start(path + "\\Шаблоны\\Согласие-шаблон.docx");
            }
        }

        private void CreateShedulePays(PrintContract data, DocX document, double sum)
        {
            for (int a = 0; a < 2; a++)
            {
                // форматирование заголовка
                Formatting titleFormat = new Formatting();
                titleFormat.FontFamily = new Font("Calibri");
                titleFormat.Size = 10D;
                titleFormat.Bold = true;

                //форматирование текста
                Formatting textFormat = new Formatting();
                textFormat.FontFamily = new Font("Calibri");
                textFormat.Size = 10D;
                textFormat.Italic = false;

                if (a == 0)
                    document.Paragraphs[document.Paragraphs.Count - 1].InsertPageBreakAfterSelf();
                else
                    document.InsertParagraph("", false, titleFormat);

                // вставка заголовка
                string text = "Приложение № 1";
                var title = document.InsertParagraph(text, false, titleFormat);
                title.Alignment = Alignment.center;
                title.SpacingAfter(10D);

                text = String.Format("к договору № _______________ от {0} года", DateTime.Now.ToShortDateString());
                title = document.InsertParagraph(text, false, titleFormat);
                title.Alignment = Alignment.center;
                title.SpacingAfter(10D);

                text = "на оказание платных дополнительных образовательных услуг по программе:";
                title = document.InsertParagraph(text, false, textFormat);
                title.Alignment = Alignment.left;

                text = data.Direction.Name;
                title = document.InsertParagraph(text, false, titleFormat);
                title.Alignment = Alignment.left;

                title = document.InsertParagraph("", false, titleFormat);
                title.Alignment = Alignment.center;

                var count = data.Schedules.Count;
                var table = document.AddTable(count + 1, 5);
                table.Alignment = Alignment.center;
                table.Design = TableDesign.TableGrid;

                for (int i = 0; i < table.RowCount; i++)
                {
                    if (i == 0)
                    {
                        var c = table.Rows[i].Cells[0].Paragraphs.First().Append("№").Bold().FontSize(10);
                        c.Alignment = Alignment.center;
                        c = table.Rows[i].Cells[1].Paragraphs.First().Append("Месяц обучения").Bold().FontSize(10);
                        c.Alignment = Alignment.center;
                        c = table.Rows[i].Cells[2].Paragraphs.First().Append("Дата оплаты").Bold().FontSize(10);
                        c.Alignment = Alignment.center;
                        c = table.Rows[i].Cells[3].Paragraphs.First().Append("Кол-во ак.ч.").Bold().FontSize(10);
                        c.Alignment = Alignment.center;
                        c = table.Rows[i].Cells[4].Paragraphs.First().Append("Сумма (руб.)").Bold().FontSize(10);
                        c.Alignment = Alignment.center;
                    }
                    else
                    {
                        var c1 = table.Rows[i].Cells[0].Paragraphs.First().Append(i.ToString()).FontSize(10);
                        c1.Alignment = Alignment.center;
                        table.Rows[i].Cells[1].Paragraphs.First().Append(data.Schedules[i - 1].Date.ToString("MMMM")).FontSize(10);
                        c1 = table.Rows[i].Cells[2].Paragraphs.First().Append(data.Schedules[i - 1].Date.ToShortDateString()).FontSize(10);
                        c1.Alignment = Alignment.center;
                        c1 = table.Rows[i].Cells[3].Paragraphs.First().Append((data.Group.DurationCourse / count).ToString()).FontSize(10);
                        c1.Alignment = Alignment.center;
                        c1 = table.Rows[i].Cells[4].Paragraphs.First().Append(data.Schedules[i - 1].Val.ToString("0.00")).FontSize(10);
                        c1.Alignment = Alignment.center;
                    }
                }
                table.AutoFit = AutoFit.Contents;
                document.InsertTable(table);

                document.InsertParagraph("", false, titleFormat);

                text = String.Format($"Полная стоимость дополнительной образовательной услуги по договору за период с " +
                    $"{data.Schedules.First().Date.ToShortDateString()} г. по " +
                    $"{(data.Schedules.Last().Date.AddMonths(1).AddDays(-1)).ToShortDateString()} г. составляет:");
                title = document.InsertParagraph(text, false, textFormat);
                title.Alignment = Alignment.left;

                //вывод суммы
                NumeralsFormatter formatter = new NumeralsFormatter();
                formatter.CultureInfo = new CultureInfo("ru-Ru");
                string format = "{0:T;M}";
                var valueCourse = String.Format(formatter, format, (int)sum, 0);

                titleFormat.UnderlineStyle = UnderlineStyle.singleLine;
                text = String.Format($"{sum.ToString()} ({valueCourse}) рублей.");
                title = document.InsertParagraph(text, false, titleFormat);
                title.Alignment = Alignment.center;

                titleFormat.UnderlineStyle = UnderlineStyle.none;
                titleFormat.Italic = true;
                titleFormat.Bold = false;
                text = "сумма прописью";
                title = document.InsertParagraph(text, false, titleFormat);
                title.Alignment = Alignment.center;
                title.SpacingAfter(8D);

                //подписи
                var table1 = document.AddTable(1, 2);
                table1.SetWidths(new float[] { 7.5F, 7.5F });
                table1.Alignment = Alignment.center;
                table1.Design = TableDesign.None;

                var c2 = table1.Rows[0].Cells[0].Paragraphs.First().Append("Исполнитель:").Bold();
                c2.AppendLine();
                c2.AppendLine("Директор _______________ Т.Г.Кудрейко");
                var c3 = table1.Rows[0].Cells[1].Paragraphs.First().Append("Заказчик:").Bold();
                c3.AppendLine();
                c3.AppendLine("___________ /__________________________");
                c3.AppendLine("        (подпись)                              (ФИО)").FontSize(8D);
                table1.AutoFit = AutoFit.Contents;
                document.InsertTable(table1);

                if (a == 0)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        document.InsertParagraph("", false, titleFormat);
                    }

                    document.InsertParagraph("____________________________________________________________________________________________________", false, titleFormat);
                }
            }
        }

        public void ExportExcelStudent(DataGridView dgv, bool total)
        {
            try
            {
                using (ExcelPackage eP = new ExcelPackage())
                {
                    eP.Workbook.Properties.Author = "ИСТРА";
                    eP.Workbook.Properties.Title = "Список учащихся";
                    eP.Workbook.Properties.Company = "ИСТРА";

                    var sheet = eP.Workbook.Worksheets.Add("Список учащихся");
                    var row = 1;
                    var col = 1;

                    // шапка                
                    List<string> columnsVisible = new List<string>();
                    columnsVisible.Add("№");
                    //создание столбца с нумерацией
                    sheet.Cells[row, col].Value = "№";
                    //создание остальных столбцов шапки
                    foreach (DataGridViewColumn column in dgv.Columns)
                    {
                        if (column.Visible != false)
                        {
                            columnsVisible.Add(column.Name);
                            sheet.Cells[row, ++col].Value = column.HeaderText;
                        }
                    }
                    //оформление шапки
                    using (var cells = sheet.Cells[sheet.Dimension.Address])
                    {
                        cells.Style.Font.Bold = true;
                        cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    row++;
                    col = 1;
                    //заполнение таблицы данными
                    foreach (String c in columnsVisible)
                    {
                        if (c == "№")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = a + 1;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c == "NameGroup" || c == "Sex" || c == "School" || c == "Class" || c == "Shift" || c == "Year")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c == "Lastname" || c == "Firstname" || c == "Middlename" || c == "Status" || c == "Note" || c == "Privilege" || c == "Teacher" || c == "Cause")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                        else if (c.IndexOf("Phone") > -1)
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                if (Convert.ToString(dgv.Rows[a].Cells[c].Value) != "")
                                {
                                    string phone = dgv.Rows[a].Cells[c].Value.ToString().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                                    long num = 0;
                                    if (phone != "")
                                    {
                                        num = Convert.ToInt64(phone);
                                        sheet.Cells[a + row, col].Value = num;
                                        sheet.Cells[a + row, col].Style.Numberformat.Format = "(###) ###-####";
                                        sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    }
                                }
                            }
                        else if (c.IndexOf("Date") > -1 || c == "Begin")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Style.Numberformat.Format = "dd/MM/yyyy";
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c.IndexOf("Accrual") > -1 || c == "Payment" || c == "Saldo")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                        col++;
                    }

                    //подведение итогов
                    if (total)
                    {
                        sheet.Cells[dgv.Rows.Count + 2, 12].Style.Numberformat.Format = "###,###,##0.00";
                        sheet.Cells[dgv.Rows.Count + 2, 12].Formula = "SUM(L2:L" + (dgv.Rows.Count + 1).ToString() + ")";
                        sheet.Cells[dgv.Rows.Count + 2, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        sheet.Cells[dgv.Rows.Count + 2, 13].Style.Numberformat.Format = "###,###,##0.00";
                        sheet.Cells[dgv.Rows.Count + 2, 13].Formula = "SUM(M2:M" + (dgv.Rows.Count + 1).ToString() + ")";
                        sheet.Cells[dgv.Rows.Count + 2, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        sheet.Cells[dgv.Rows.Count + 2, 14].Style.Numberformat.Format = "###,###,##0.00";
                        sheet.Cells[dgv.Rows.Count + 2, 14].Formula = "SUM(N2:N" + (dgv.Rows.Count + 1).ToString() + ")";
                        sheet.Cells[dgv.Rows.Count + 2, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        sheet.Cells[dgv.Rows.Count + 2, 15].Style.Numberformat.Format = "###,###,##0.00";
                        sheet.Cells[dgv.Rows.Count + 2, 15].Formula = "SUM(O2:O" + (dgv.Rows.Count + 1).ToString() + ")";
                        sheet.Cells[dgv.Rows.Count + 2, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        sheet.Cells[dgv.Rows.Count + 2, 16].Value = "Итого";
                        sheet.Cells[dgv.Rows.Count + 2, 16].Style.Font.Bold = true;

                        sheet.Cells[dgv.Rows.Count + 3, 15].Style.Numberformat.Format = "###,###,##0.00";
                        sheet.Cells[dgv.Rows.Count + 3, 15].Formula = "SUMIF(O2:O" + (dgv.Rows.Count + 1).ToString() + ", \">0\")";
                        sheet.Cells[dgv.Rows.Count + 3, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        sheet.Cells[dgv.Rows.Count + 3, 16].Value = "Долг";
                        sheet.Cells[dgv.Rows.Count + 3, 16].Style.Font.Bold = true;

                        sheet.Cells[dgv.Rows.Count + 4, 15].Style.Numberformat.Format = "###,###,##0.00";
                        sheet.Cells[dgv.Rows.Count + 4, 15].Formula = "-1*SUMIF(O2:O" + (dgv.Rows.Count + 1).ToString() + ", \"<0\")";
                        sheet.Cells[dgv.Rows.Count + 4, 15].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        sheet.Cells[dgv.Rows.Count + 4, 16].Value = "Аванс";
                        sheet.Cells[dgv.Rows.Count + 4, 16].Style.Font.Bold = true;
                    }

                    //оформление данных
                    using (var cells = sheet.Cells[sheet.Dimension.Address])
                    {
                        cells.AutoFitColumns();
                    }

                    // сохраняем в файл
                    Directory.CreateDirectory($"Отчеты");
                    var bin = eP.GetAsByteArray();
                    try
                    {
                        File.WriteAllBytes(@"Отчеты/Список слушателей-" + DateTime.Now.ToShortDateString() + ".xlsx", bin);
                        var file = new FileInfo(@"Отчеты/Список слушателей-" + DateTime.Now.ToShortDateString() + ".xlsx");
                        Process.Start(file.FullName);
                    }
                    catch (IOException ex)
                    {
                        CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                        MessageBox.Show("Файл со списком слушателей не может быть создан. Если файл уже создавался сегодня и открыт, закройте его и повторите попытку.",
                            "Ошибка создания файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                        MessageBox.Show($"При открытии файла произошла ошибка:\n{ex.Message}", "Ошибка открытия файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ExportExcelStudentFull(List<ListStudentFull> listStudentFull)
        {
            try
            {
                using (ExcelPackage eP = new ExcelPackage())
                {
                    eP.Workbook.Properties.Author = "ИСТРА";
                    eP.Workbook.Properties.Title = "Подробный список учащихся";
                    eP.Workbook.Properties.Company = "ИСТРА";

                    var sheet = eP.Workbook.Worksheets.Add("Подробный список учащихся");

                    var row = 1;
                    var col = 1;

                    // шапка                
                    List<string> columnsVisible = new List<string>();
                    columnsVisible.Add("№");
                    //создание столбцов
                    sheet.Cells[row, col].Value = "№";
                    sheet.Cells[row, ++col].Value = "Фамилия";
                    sheet.Cells[row, ++col].Value = "Имя";
                    sheet.Cells[row, ++col].Value = "Отчество";
                    sheet.Cells[row, ++col].Value = "Дата рождения";
                    sheet.Cells[row, ++col].Value = "Возраст";
                    sheet.Cells[row, ++col].Value = "Пол";
                    sheet.Cells[row, ++col].Value = "Город";
                    sheet.Cells[row, ++col].Value = "Адрес";
                    sheet.Cells[row, ++col].Value = "Статус учащегося";
                    sheet.Cells[row, ++col].Value = "Школа";
                    sheet.Cells[row, ++col].Value = "Класс";
                    sheet.Cells[row, ++col].Value = "Смена";
                    sheet.Cells[row, ++col].Value = "Тел1";
                    sheet.Cells[row, ++col].Value = "Тел2";
                    sheet.Cells[row, ++col].Value = "Фамилия родителя";
                    sheet.Cells[row, ++col].Value = "Имя родителя";
                    sheet.Cells[row, ++col].Value = "Отчество родителя";
                    sheet.Cells[row, ++col].Value = "Тел. родителя";
                    sheet.Cells[row, ++col].Value = "Группа";
                    sheet.Cells[row, ++col].Value = "Учеб. год";
                    sheet.Cells[row, ++col].Value = "Статус группы";
                    sheet.Cells[row, ++col].Value = "Сумма начислений";
                    sheet.Cells[row, ++col].Value = "Сумма оплат";
                    sheet.Cells[row, ++col].Value = "Сальдо";
                    //оформление шапки
                    using (var cells = sheet.Cells[sheet.Dimension.Address])
                    {
                        cells.Style.Font.Bold = true;
                        cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    row++;
                    col = 1;
                    //заполнение таблицы данными
                    //получение списка полей класса 
                    Type t = typeof(ListStudentFull);
                    PropertyInfo[] propInfos = t.GetProperties();
                    int count = listStudentFull.Count;
                    foreach (var c in propInfos)
                    {
                        if (c.Name == "EnrollmentId")
                            for (int a = 0; a < count; a++)
                            {
                                sheet.Cells[a + row, col].Value = a + 1;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c.Name == "YearId") { col--; }
                        else if (c.Name == "Group" || c.Name == "Sex" || c.Name == "School" || c.Name == "Class" || c.Name == "Shift" || c.Name == "Age" || c.Name == "Year")
                            for (int a = 0; a < count; a++)
                            {
                                sheet.Cells[a + row, col].Value = c.GetValue(listStudentFull[a]);
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c.Name == "Lastname" || c.Name == "Firstname" || c.Name == "Middlename" || c.Name.IndexOf("Status") > -1
                            || c.Name == "LastnameParent" || c.Name == "FirstnameParent" || c.Name == "MiddlenameParent" || c.Name == "City" || c.Name =="Address")
                            for (int a = 0; a < count; a++)
                            {
                                sheet.Cells[a + row, col].Value = c.GetValue(listStudentFull[a]);
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                        else if (c.Name.IndexOf("Phone") > -1)
                            for (int a = 0; a < count; a++)
                            {
                                if (Convert.ToString(c.GetValue(listStudentFull[a])) != "")
                                {
                                    string phone = c.GetValue(listStudentFull[a]).ToString().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                                    long num = 0;
                                    if (phone != "")
                                    {
                                        num = Convert.ToInt64(phone);
                                        sheet.Cells[a + row, col].Value = num;
                                        sheet.Cells[a + row, col].Style.Numberformat.Format = "(###) ###-####";
                                        sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    }
                                }
                            }
                        else if (c.Name.IndexOf("Date") > -1)
                            for (int a = 0; a < count; a++)
                            {
                                sheet.Cells[a + row, col].Style.Numberformat.Format = "dd/MM/yyyy";
                                sheet.Cells[a + row, col].Value = c.GetValue(listStudentFull[a]);
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c.Name.IndexOf("Sum") > -1)
                            for (int a = 0; a < count; a++)
                            {
                                double sum = (c.GetValue(listStudentFull[a]).ToString() == "") ? 0 : Convert.ToDouble(c.GetValue(listStudentFull[a]));
                                sheet.Cells[a + row, col].Style.Numberformat.Format = "0.00";
                                sheet.Cells[a + row, col].Value = sum;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                        else if (c.Name == "SaldoNow")
                            for (int a = 0; a < count; a++)
                            {
                                sheet.Cells[a + row, col].Style.Numberformat.Format = "0.00";
                                sheet.Cells[a + row, col].Formula = "X" + (a + row) + "-W" + (a + row);
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                        col++;
                    }
                    //оформление данных
                    using (var cells = sheet.Cells[sheet.Dimension.Address])
                    {
                        cells.AutoFitColumns();
                    }

                    // сохраняем в файл
                    Directory.CreateDirectory($"Отчеты");
                    var bin = eP.GetAsByteArray();
                    try
                    {
                        File.WriteAllBytes(@"Отчеты/Подробный список учащихся-" + DateTime.Now.ToShortDateString() + ".xlsx", bin);
                        var file = new FileInfo(@"Отчеты/Подробный список учащихся-" + DateTime.Now.ToShortDateString() + ".xlsx");
                        Process.Start(file.FullName);
                    }
                    catch (IOException ex)
                    {
                        CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                        MessageBox.Show("Файл со списком слушателей не может быть создан. Если файл уже создавался сегодня и открыт, закройте его и повторите попытку.",
                            "Ошибка создания файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                        MessageBox.Show($"При открытии файла произошла ошибка:\n{ex.Message}", "Ошибка открытия файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ExportExcelGroup(DataGridView dgv, bool total)
        {
            try
            {
                using (ExcelPackage eP = new ExcelPackage())
                {
                    eP.Workbook.Properties.Author = "ИСТРА";
                    eP.Workbook.Properties.Title = "Список групп";
                    eP.Workbook.Properties.Company = "ИСТРА";

                    var sheet = eP.Workbook.Worksheets.Add("Список групп");

                    var row = 1;
                    var col = 1;

                    // шапка                
                    List<string> columnsVisible = new List<string>();
                    columnsVisible.Add("№");
                    //создание столбца с нумерацией
                    sheet.Cells[row, col].Value = "№";
                    //создание остальных столбцов шапки
                    foreach (DataGridViewColumn column in dgv.Columns)
                    {
                        if (column.Visible != false)
                        {
                            columnsVisible.Add(column.Name);
                            sheet.Cells[row, ++col].Value = column.HeaderText;
                        }
                    }
                    //оформление шапки
                    using (var cells = sheet.Cells[sheet.Dimension.Address])
                    {
                        cells.Style.Font.Bold = true;
                        cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    row++;
                    col = 1;
                    //заполнение таблицы данными
                    foreach (String c in columnsVisible)
                    {
                        if (c == "№")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = a + 1;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c == "DurationLesson" || c == "Days" || c == "Year" || c == "TwoTeachers" || c == "Students")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c == "Direction" || c == "Name" || c == "Teacher" || c == "Note" || c == "Branch" || c == "Class" || c == "CourseName")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                        else if (c == "Begin")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Style.Numberformat.Format = "HH:mm";
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c == "Price" || c == "PlanAccrual" || c == "Accrual" || c == "AccrualDiscount" || c == "Payment" || c == "Saldo")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Style.Numberformat.Format = "0.00";
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c == "StudentsP" || c == "StudentsF")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        col++;
                    }

                    if (total)
                    {
                        sheet.Cells[dgv.Rows.Count + 2, 9].Style.Numberformat.Format = "###,###,##0.00";
                        sheet.Cells[dgv.Rows.Count + 2, 9].Formula = "SUM(I1:I" + (dgv.Rows.Count + 1).ToString() + ")";
                        sheet.Cells[dgv.Rows.Count + 2, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        sheet.Cells[dgv.Rows.Count + 2, 10].Style.Numberformat.Format = "###,###,##0.00";
                        sheet.Cells[dgv.Rows.Count + 2, 10].Formula = "SUM(J1:J" + (dgv.Rows.Count + 1).ToString() + ")";
                        sheet.Cells[dgv.Rows.Count + 2, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        sheet.Cells[dgv.Rows.Count + 2, 11].Style.Numberformat.Format = "###,###,##0.00";
                        sheet.Cells[dgv.Rows.Count + 2, 11].Formula = "SUM(K1:K" + (dgv.Rows.Count + 1).ToString() + ")";
                        sheet.Cells[dgv.Rows.Count + 2, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        sheet.Cells[dgv.Rows.Count + 2, 12].Style.Numberformat.Format = "###,###,##0.00";
                        sheet.Cells[dgv.Rows.Count + 2, 12].Formula = "SUM(L1:L" + (dgv.Rows.Count + 1).ToString() + ")";
                        sheet.Cells[dgv.Rows.Count + 2, 12].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        sheet.Cells[dgv.Rows.Count + 2, 13].Style.Numberformat.Format = "###,###,##0.00";
                        sheet.Cells[dgv.Rows.Count + 2, 13].Formula = "SUM(M1:M" + (dgv.Rows.Count + 1).ToString() + ")";
                        sheet.Cells[dgv.Rows.Count + 2, 13].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        sheet.Cells[dgv.Rows.Count + 2, 14].Value = "Итого";
                        sheet.Cells[dgv.Rows.Count + 2, 14].Style.Font.Bold = true;
                    }
                    //оформление данных
                    using (var cells = sheet.Cells[sheet.Dimension.Address])
                    {
                        cells.AutoFitColumns();
                    }

                    // сохраняем в файл
                    Directory.CreateDirectory($"Отчеты");
                    var bin = eP.GetAsByteArray();
                    try
                    {
                        File.WriteAllBytes(@"Отчеты/Список групп-" + DateTime.Now.ToShortDateString() + ".xlsx", bin);
                        var file = new FileInfo(@"Отчеты/Список групп-" + DateTime.Now.ToShortDateString() + ".xlsx");
                        Process.Start(file.FullName);
                    }
                    catch (IOException ex)
                    {
                        CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                        MessageBox.Show("Файл со списком групп не может быть создан. Если файл уже создавался сегодня и открыт, закройте его и повторите попытку.",
                            "Ошибка создания файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                        MessageBox.Show($"При открытии файла произошла ошибка:\n{ex.Message}", "Ошибка открытия файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void ExportExcelGroupFull(List<ListGroupFull> listGroupFull)
        {
            try
            {
                using (ExcelPackage eP = new ExcelPackage())
                {
                    eP.Workbook.Properties.Author = "ИСТРА";
                    eP.Workbook.Properties.Title = "Подробный список групп";
                    eP.Workbook.Properties.Company = "ИСТРА";

                    var sheet = eP.Workbook.Worksheets.Add("Подробный список групп");

                    var row = 1;
                    var col = 1;

                    // шапка                
                    List<string> columnsVisible = new List<string>();
                    columnsVisible.Add("№");
                    //создание столбцов
                    sheet.Cells[row, col].Value = "№";
                    sheet.Cells[row, ++col].Value = "Направление";
                    sheet.Cells[row, ++col].Value = "Курс";
                    sheet.Cells[row, ++col].Value = "Группа";
                    sheet.Cells[row, ++col].Value = "Статус";
                    sheet.Cells[row, ++col].Value = "Корпус";
                    sheet.Cells[row, ++col].Value = "Класс";
                    sheet.Cells[row, ++col].Value = "Дни";
                    sheet.Cells[row, ++col].Value = "Начало занятия";
                    sheet.Cells[row, ++col].Value = "Продол. занятия";
                    sheet.Cells[row, ++col].Value = "Продол. курса";
                    sheet.Cells[row, ++col].Value = "Кол-во занятий";
                    sheet.Cells[row, ++col].Value = "Прошло занятий";
                    sheet.Cells[row, ++col].Value = "Преподаватель";
                    sheet.Cells[row, ++col].Value = "Два препод.";
                    sheet.Cells[row, ++col].Value = "Кол-во учащихся";
                    sheet.Cells[row, ++col].Value = "Учеб. год";
                    sheet.Cells[row, ++col].Value = "Сумма начислений";
                    sheet.Cells[row, ++col].Value = "Сумма оплат";
                    sheet.Cells[row, ++col].Value = "Сальдо";
                    //оформление шапки
                    using (var cells = sheet.Cells[sheet.Dimension.Address])
                    {
                        cells.Style.Font.Bold = true;
                        cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    row++;
                    col = 1;
                    //заполнение таблицы данными
                    //получение списка полей класса 
                    Type t = typeof(ListGroupFull);
                    PropertyInfo[] propInfos = t.GetProperties();
                    int count = listGroupFull.Count;
                    foreach (var c in propInfos)
                    {
                        if (c.Name == "Id")
                            for (int a = 0; a < count; a++)
                            {
                                sheet.Cells[a + row, col].Value = a + 1;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c.Name == "StatusGroup" || c.Name == "Branch" || c.Name == "Class" || c.Name == "Days" || c.Name == "Begin" || c.Name == "TwoTeachers" || c.Name == "Year")
                            for (int a = 0; a < count; a++)
                            {
                                sheet.Cells[a + row, col].Value = Convert.ToString(c.GetValue(listGroupFull[a]));
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c.Name == "DurationLesson" || c.Name == "DurationCourse" || c.Name == "CountLesson" || c.Name == "PassedLesson" || c.Name == "Students")
                            for (int a = 0; a < count; a++)
                            {
                                sheet.Cells[a + row, col].Value = c.GetValue(listGroupFull[a]);
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c.Name == "Direction" || c.Name == "Course" || c.Name == "Teacher" || c.Name == "Group")
                            for (int a = 0; a < count; a++)
                            {
                                sheet.Cells[a + row, col].Value = c.GetValue(listGroupFull[a]);
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                        else if (c.Name.IndexOf("Sum") > -1)
                            for (int a = 0; a < count; a++)
                            {
                                double sum = (c.GetValue(listGroupFull[a]).ToString() == "") ? 0 : Convert.ToDouble(c.GetValue(listGroupFull[a]));
                                sheet.Cells[a + row, col].Style.Numberformat.Format = "0.00";
                                sheet.Cells[a + row, col].Value = sum;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                        else if (c.Name == "SaldoNow")
                            for (int a = 0; a < count; a++)
                            {
                                sheet.Cells[a + row, col].Style.Numberformat.Format = "0.00";
                                sheet.Cells[a + row, col].Formula = "R" + (a + row) + "-S" + (a + row);
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                        col++;
                    }
                    //оформление данных
                    using (var cells = sheet.Cells[sheet.Dimension.Address])
                    {
                        cells.AutoFitColumns();
                    }

                    // сохраняем в файл
                    Directory.CreateDirectory($"Отчеты");
                    var bin = eP.GetAsByteArray();
                    try
                    {
                        File.WriteAllBytes(@"Отчеты/Подробный список групп-" + DateTime.Now.ToShortDateString() + ".xlsx", bin);

                        var file = new FileInfo(@"Отчеты/Подробный список групп-" + DateTime.Now.ToShortDateString() + ".xlsx");
                        Process.Start(file.FullName);
                    }
                    catch (IOException ex)
                    {
                        CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                        MessageBox.Show("Файл со списком групп не может быть создан. Если файл уже создавался сегодня и открыт, закройте его и повторите попытку.",
                            "Ошибка создания файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                        MessageBox.Show($"При открытии файла произошла ошибка:\n{ex.Message}", "Ошибка открытия файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Receipts(List<Payment> pays, DateTime date, string paymaster, bool enableDate)
        {
            try
            {
                //создание копии квитанции
                string path = Directory.GetCurrentDirectory() + "\\";
                File.Copy(path + @"Шаблоны/Квитанция-шаблон.xlsx", path + @"Шаблоны/Квитанции-" + System.Environment.MachineName + ".xlsx", true);

                //работа с квитанцией
                //открытие файла
                FileInfo newFile = new FileInfo(@"Шаблоны/Квитанции-" + System.Environment.MachineName + ".xlsx");
                ExcelPackage pck = new ExcelPackage(newFile);
                var ws = pck.Workbook.Worksheets[1];

                ws.PrinterSettings.PrintArea = ws.Cells["A: DJ"];
                int totalRows = 39, totalCols = 116;
                for (int i = 0; i < pays.Count; i++)
                {
                    //копирование квитанции для нескольких платежей
                    if (i > 0)
                    {
                        for (int j = 1; j <= 39; j++)
                        {
                            double b = ws.Row(j).Height;
                            if (b == 15) b = 11.25;//строка выше не всегда передает правильную высоту строки, поэтому корректируем
                            ws.Row((i * totalRows) + j).Height = b;
                        }
                        ws.Cells[1, 1, totalRows, totalCols].Copy(ws.Cells[i * totalRows + 1, 1]);
                    }

                    //Вставка даты платежа
                    if (enableDate)
                    {
                        ws.Cells[13, 54].Value = date.ToShortDateString();
                        ws.Cells[9, 79].Value = date.Day;
                        ws.Cells[28, 77].Value = date.Day;
                        ws.Cells[9, 85].Value = CultureInfo.CurrentCulture.DateTimeFormat.MonthGenitiveNames[date.Month - 1];
                        ws.Cells[28, 83].Value = CultureInfo.CurrentCulture.DateTimeFormat.MonthGenitiveNames[date.Month - 1];
                        ws.Cells[9, 101].Value = date.Year;
                        ws.Cells[28, 99].Value = date.Year;
                    }
                    //Вставка суммы
                    ws.Cells[19, 39].Value = String.Format("{0} руб. 00коп.", pays[i].ValuePayment);
                    ws.Cells[21, 81].Value = pays[i].ValuePayment;
                    //прописью
                    NumeralsFormatter formatter = new NumeralsFormatter();
                    formatter.CultureInfo = new CultureInfo("ru-Ru");
                    int value = (int)pays[i].ValuePayment;
                    string format = "{0:t;f}";
                    ws.Cells[30, 7].Value = String.Format(formatter, format, value);
                    format = "{0:t;f}";
                    ws.Cells[23, 75].Value = String.Format(formatter, format, value);

                    using (IstraContext db = new IstraContext())
                    {
                        //Вставка данных плательщика
                        var enrollInfo = db.Enrollments.Find(pays[i].EnrollmentId);
                        var studentInfo = db.Students.Find(enrollInfo.StudentId);
                        var groupInfo = db.Groups.Find(enrollInfo.GroupId);
                        var cityInfo = db.Cities.Find(studentInfo.CityId);
                        if (cityInfo == null)
                        {
                            //MessageBox.Show("Отсутствуют данные о городе.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //return;
                            cityInfo = new City();
                            cityInfo.Name = "         ";
                        }
                        var streetInfo = db.Streets.Find(studentInfo.StreetId);
                        if (streetInfo == null)
                        {
                            //MessageBox.Show("Отсутствуют данные об адресе проживания.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //return;
                            streetInfo = new Street();
                            streetInfo.Name = "       ";
                        }

                        //ФИО
                        if (studentInfo != null)
                        {
                            if (studentInfo.LastnameParent != null)
                            {
                                ws.Cells[21, 10].Value = studentInfo.FullnameParent();
                                ws.Cells[10, 84].Value = studentInfo.LastnameParent + " " + studentInfo.FirstnameParent;
                                ws.Cells[11, 75].Value = studentInfo.MiddlenameParent;
                            }
                            else
                            {
                                ws.Cells[21, 10].Value = studentInfo.Fullname();
                                ws.Cells[10, 84].Value = studentInfo.Lastname + " " + studentInfo.Firstname;
                                ws.Cells[11, 75].Value = studentInfo.Middlename;
                            }
                        }

                        //Адрес
                        string @float = (studentInfo.Float != null) ? " кв. " + studentInfo.Float : "";
                        ws.Cells[23, 7].Value = "453100 РБ г." + cityInfo.Name + " ул. " + streetInfo.Name + " д. " + studentInfo.House + @float;
                        ws.Cells[12, 81].Value = "453100 РБ г. " + cityInfo.Name;
                        ws.Cells[13, 75].Value = "ул. " + streetInfo.Name + " д. " + studentInfo.House + @float;

                        //Паспорт
                        ws.Cells[25, 8].Value = studentInfo.GetPassport();
                        ws.Cells[14, 82].Value = studentInfo.GetPassportNumber();
                        ws.Cells[15, 75].Value = studentInfo.GetPassportIssuedBy();

                        //Основание
                        ws.Cells[27, 10].Value = "оплата обучения " + studentInfo.Fullname() + " за " +
                            CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName((int)pays[i].MonthId) + " " +
                            pays[i].Year + "г.";
                        ws.Cells[28, 1].Value = "группа: " + groupInfo.Name;
                        ws.Cells[16, 84].Value = "оплата обучения ";
                        ws.Cells[17, 75].Value = studentInfo.Fullname();
                        ws.Cells[18, 75].Value = " за " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName((int)pays[i].MonthId) + " " +
                            pays[i].Year + "г. группа: " + groupInfo.Name;

                        //кассир
                        ws.Cells[36, 34].Value = paymaster;
                        ws.Cells[36, 92].Value = paymaster;
                    }
                    if (i % 2 == 0)
                    {
                        var modelCells = ws.Cells[38, 1, 38, totalCols];
                        modelCells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                    else
                    {
                        var modelCells = ws.Cells[38, 1, 38, totalCols];
                        modelCells.Style.Border.Bottom.Style = ExcelBorderStyle.None;
                        ws.Row(78 * i).PageBreak = true;
                    }
                }

                //сохранение и открытие файла
                var bin = pck.GetAsByteArray();
                File.WriteAllBytes(@"Шаблоны/Квитанции-" + Environment.MachineName + ".xlsx", bin);
                var file = new FileInfo(@"Шаблоны/Квитанции-" + Environment.MachineName + ".xlsx");
                Process.Start(file.FullName);
            }
            catch (IOException ex)
            {
                CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                MessageBox.Show("Файл квитанции не может быть создан. Если файл уже создавался сегодня и открыт, закройте его и повторите попытку.",
                    "Ошибка создания файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                MessageBox.Show($"При открытии файла произошла ошибка:\n{ex.Message}", "Ошибка открытия файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void PaySheet(int[] workersId, DateTime period)
        {
            //получение периода операций
            var dateStart = new DateTime(period.Year, period.Month, 1);
            var dateEnd = dateStart.AddMonths(1);
            //получение данных о работнике

            var currentWorker = new Worker();


            //создание копии квитанции
            string path = Directory.GetCurrentDirectory() + "\\";
            File.Copy(path + @"Шаблоны/Расчетный-лист-шаблон.xlsx", path + @"Шаблоны/Расчетный-лист-" + System.Environment.MachineName + ".xlsx", true);

            //работа с квитанцией
            //открытие файла
            FileInfo newFile = new FileInfo(@"Шаблоны/Расчетный-лист-" + Environment.MachineName + ".xlsx");
            ExcelPackage pck = new ExcelPackage(newFile);
            var ws = pck.Workbook.Worksheets[1];

            ws.PrinterSettings.PrintArea = ws.Cells["A: AI"];
            int totalRows = 15, totalCols = 35;
            int[] indexRowStartTransactionList = new int[workersId.Length];
            indexRowStartTransactionList[0] = 0;
            for (int i = 0; i < workersId.Length; i++)
            {
                //копирование расчетки для нескольких сотрудников
                if (i > 0)
                {
                    for (int j = 1; j <= totalRows; j++)
                    {
                        double b = ws.Row(j).Height;
                        ws.Row((i * totalRows) + j).Height = b;
                    }
                    ws.Cells[1, 1, totalRows, totalCols].Copy(ws.Cells[i * totalRows + 1, 1]);
                }
            }

            int insertedRows = 0;
            for (int i = 0; i < workersId.Length; i++)
            {
                var transactionsProfit = new List<ListTransactions>();
                var transactionsRetention = new List<ListTransactions>();
                var transactionsPayment = new List<ListTransactions>();

                //получение данных о преподавателе и операциях
                using (IstraContext db = new IstraContext())
                {
                    //получение начислений за период
                    int tempWorkerId = workersId[i];
                    currentWorker = db.Workers.Include(a => a.Department).Include(a => a.Post).FirstOrDefault(a => a.Id == tempWorkerId);

                    transactionsProfit = db.Retentions.Include(a => a.Base).Include(a => a.TypeOfTransaction)
                        .Where(a => a.Month >= dateStart && a.Month < dateEnd && a.WorkerId == tempWorkerId && a.TypeOfTransaction.Name == "Начисление")
                            .Select(a => new
                            ListTransactions
                            {
                                Name = a.BaseId != null ? a.Base.Name : a.Name,
                                Date = a.Date,
                                Period = dateStart,
                                Count = a.Count != null ? a.Count : null,
                                Hours = a.Hours != null ? a.Hours : null,
                                Wage = a.Wage != null ? a.Wage : null,
                                Total = a.Value
                            }).ToList();

                    //получение удержаний за период
                    transactionsRetention = db.Retentions.Include(a => a.Base).Include(a => a.TypeOfTransaction)
                        .Where(a => a.Month >= dateStart && a.Month < dateEnd && a.WorkerId == tempWorkerId && a.TypeOfTransaction.Name == "Удержание")
                            .Select(a => new
                            ListTransactions
                            {
                                Name = a.Base.Name,
                                Date = a.Date,
                                Period = dateStart,
                                Count = null,
                                Hours = null,
                                Wage = null,
                                Total = a.Value
                            }).ToList();

                    //получение выплат за период
                    transactionsPayment = db.Retentions.Include(a => a.Base).Include(a => a.TypeOfTransaction)
                        .Where(a => a.Month >= dateStart && a.Month < dateEnd && a.WorkerId == tempWorkerId && a.TypeOfTransaction.Name == "Выплата")
                            .Select(a => new
                            ListTransactions
                            {
                                Name = a.Base.Name,
                                Date = a.Date,
                                Period = dateStart,
                                Count = null,
                                Hours = null,
                                Wage = null,
                                Total = a.Value
                            }).ToList();
                }

                //заполнение шапки расчетки
                ws.Cells[2 + indexRowStartTransactionList[i], 8].Value = period.ToString("MMMM yyyy");
                ws.Cells[3 + indexRowStartTransactionList[i], 1].Value = String.Format($"{currentWorker.Lastname} {currentWorker.Firstname} {currentWorker.Middlename}");
                ws.Cells[4 + indexRowStartTransactionList[i], 25].Value = (currentWorker.Post.Name != null) ? currentWorker.Post.Name : "";
                ws.Cells[5 + indexRowStartTransactionList[i], 5].Value = (currentWorker.Department.Name != null) ? currentWorker.Department.Name : "";


                //анализ количества строк в расчетке
                int countRows = (transactionsRetention.Count < transactionsProfit.Count - 1) ? transactionsProfit.Count : transactionsRetention.Count;
                for (int j = 2; j < countRows; j++)
                {
                    ws.InsertRow(8 + indexRowStartTransactionList[i] + j, 1);
                    insertedRows++;

                    ExcelRange r = ws.Cells["A" + (8 + j + indexRowStartTransactionList[i]) + ":G" + (8 + j + indexRowStartTransactionList[i]).ToString()];
                    r.Merge = true;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Hair;
                    r.Style.Border.Bottom.Style = r.Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    ExcelRange r1 = ws.Cells["H" + (8 + j + indexRowStartTransactionList[i]) + ":J" + (8 + j + indexRowStartTransactionList[i]).ToString()];
                    r1.Merge = true;
                    r1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    r1.Style.Border.Right.Style = ExcelBorderStyle.Hair;
                    r1.Style.Border.Bottom.Style = r1.Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    ExcelRange r2 = ws.Cells["K" + (8 + j + indexRowStartTransactionList[i]) + ":L" + (8 + j + indexRowStartTransactionList[i]).ToString()];
                    r2.Merge = true;
                    r2.Style.Border.Right.Style = ExcelBorderStyle.Hair;
                    r2.Style.Border.Bottom.Style = r2.Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    ExcelRange r3 = ws.Cells["M" + (8 + j + indexRowStartTransactionList[i]) + ":N" + (8 + j + indexRowStartTransactionList[i]).ToString()];
                    r3.Merge = true;
                    r3.Style.Border.Right.Style = ExcelBorderStyle.Hair;
                    r3.Style.Border.Bottom.Style = r3.Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    ExcelRange r4 = ws.Cells["O" + (8 + j + indexRowStartTransactionList[i]) + ":Q" + (8 + j + indexRowStartTransactionList[i]).ToString()];
                    r4.Merge = true;
                    ws.Cells["O" + (8 + j + indexRowStartTransactionList[i]) + ":Q" + (8 + j + indexRowStartTransactionList[i]).ToString()].Style.Numberformat.Format = "0.00";
                    r4.Style.Border.Right.Style = ExcelBorderStyle.Hair;
                    r4.Style.Border.Bottom.Style = r4.Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    ExcelRange r5 = ws.Cells["R" + (8 + j + indexRowStartTransactionList[i]) + ":U" + (8 + j + indexRowStartTransactionList[i]).ToString()];
                    r5.Merge = true;
                    ws.Cells["R" + (8 + j + indexRowStartTransactionList[i]) + ":U" + (8 + j + indexRowStartTransactionList[i]).ToString()].Style.Numberformat.Format = "0.00";
                    r5.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r5.Style.Border.Bottom.Style = r5.Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    ExcelRange r6 = ws.Cells["V" + (8 + j + indexRowStartTransactionList[i]) + ":AB" + (8 + j + indexRowStartTransactionList[i]).ToString()];
                    r6.Merge = true;
                    r6.Style.Border.Right.Style = ExcelBorderStyle.Hair;
                    r6.Style.Border.Bottom.Style = r6.Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    ExcelRange r7 = ws.Cells["AC" + (8 + j + indexRowStartTransactionList[i]) + ":AE" + (8 + j + indexRowStartTransactionList[i]).ToString()];
                    r7.Merge = true;
                    r7.Style.Border.Right.Style = ExcelBorderStyle.Hair;
                    r7.Style.Border.Bottom.Style = r7.Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    ExcelRange r8 = ws.Cells["AF" + (8 + j + indexRowStartTransactionList[i]) + ":AI" + (8 + j + indexRowStartTransactionList[i]).ToString()];
                    r8.Merge = true;
                    r8.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r8.Style.Border.Bottom.Style = r8.Style.Border.Top.Style = ExcelBorderStyle.Hair;
                }

                //вывод начислений в таблицу  
                int q = 0;
                int firstRow = 0, endRow = 0;
                foreach (var pr in transactionsProfit)
                {
                    if (q == 0) firstRow = 9 + indexRowStartTransactionList[i];
                    endRow = 9 + q + indexRowStartTransactionList[i];
                    ws.Cells[9 + q + indexRowStartTransactionList[i], 1].Value = pr.Name;
                    ws.Cells[9 + q + indexRowStartTransactionList[i], 8].Value = period.ToString("MMM yy");
                    ws.Cells[9 + q + indexRowStartTransactionList[i], 11].Value = pr.Count;
                    ws.Cells[9 + q + indexRowStartTransactionList[i], 13].Value = pr.Hours;
                    ws.Cells[9 + q + indexRowStartTransactionList[i], 15].Value = pr.Wage;
                    ws.Cells[9 + q + indexRowStartTransactionList[i], 18].Value = pr.Total;
                    q++;
                }
                if (firstRow != 0)
                {
                    ws.Cells[firstRow - 1, 18].Formula = "SUM(R" + firstRow + ":R" + endRow + ")";
                    firstRow = endRow = 0;
                }
                //else ws.Cells[firstRow - 1, 32].Value = 0;

                //вывод удержаний в таблицу  
                q = 0;
                foreach (var ret in transactionsRetention)
                {
                    if (q == 0) firstRow = 9 + indexRowStartTransactionList[i];
                    endRow = 9 + q + indexRowStartTransactionList[i];
                    ws.Cells[9 + q + indexRowStartTransactionList[i], 22].Value = ret.Name;
                    ws.Cells[9 + q + indexRowStartTransactionList[i], 29].Value = period.ToString("MMM yy");
                    ws.Cells[9 + q + indexRowStartTransactionList[i], 32].Value = ret.Total;
                    q++;
                }
                if (firstRow != 0)
                {
                    ws.Cells[firstRow - 1, 32].Formula = "SUM(AF" + firstRow + ":AF" + endRow + ")";
                    firstRow = endRow = 0;
                }
                //else ws.Cells[firstRow - 1, 32].Value = 0;

                //вывод выплат в таблицу
                int k = insertedRows;
                for (int j = 11 + k; j < 11 + transactionsPayment.Count + k - 1; j++)
                {
                    ws.InsertRow(insertedRows + j, 1);
                    ws.Row(insertedRows + j).Height = 21.75;

                    insertedRows++;

                    int indexRow = insertedRows + j - 1;
                    ExcelRange r9 = ws.Cells["V" + indexRow.ToString() + ":AB" + indexRow.ToString()];
                    r9.Merge = true;
                    r9.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    r9.Style.Border.Left.Style = r9.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r9.Style.Border.Right.Style = r9.Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    ExcelRange r10 = ws.Cells["AC" + indexRow.ToString() + ":AE" + indexRow.ToString()];
                    r10.Merge = true;
                    r10.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    r10.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    r10.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r10.Style.Border.Right.Style = r10.Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    ExcelRange r11 = ws.Cells["AF" + indexRow.ToString() + ":AI" + indexRow.ToString()];
                    r11.Merge = true;
                    ws.Cells["AF" + indexRow.ToString() + ":AI" + indexRow.ToString()].Style.Numberformat.Format = "0.00";
                    r11.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    r11.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                    r11.Style.Border.Bottom.Style = r11.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r11.Style.Border.Top.Style = ExcelBorderStyle.Hair;


                    ExcelRange r12 = ws.Cells["A" + indexRow.ToString() + ":G" + indexRow.ToString().ToString()];
                    r12.Merge = true;
                    r12.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r12.Style.Border.Right.Style = r12.Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    ExcelRange r13 = ws.Cells["H" + indexRow.ToString() + ":J" + indexRow.ToString().ToString()];
                    r13.Merge = true;
                    r13.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    r13.Style.Border.Right.Style = r13.Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    ExcelRange r14 = ws.Cells["K" + indexRow.ToString() + ":L" + indexRow.ToString().ToString()];
                    r14.Merge = true;
                    r14.Style.Border.Right.Style = r14.Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    ExcelRange r15 = ws.Cells["M" + indexRow.ToString() + ":N" + indexRow.ToString().ToString()];
                    r15.Merge = true;
                    r15.Style.Border.Right.Style = r15.Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    ExcelRange r16 = ws.Cells["O" + indexRow.ToString() + ":Q" + indexRow.ToString().ToString()];
                    r16.Merge = true;
                    r16.Style.Border.Right.Style = r16.Style.Border.Top.Style = ExcelBorderStyle.Hair;

                    ExcelRange r17 = ws.Cells["R" + indexRow.ToString() + ":U" + indexRow.ToString().ToString()];
                    r17.Merge = true;
                    r17.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    r17.Style.Border.Top.Style = ExcelBorderStyle.Hair;
                }
                q = 0;
                foreach (var pays in transactionsPayment)
                {
                    if (q == 0) firstRow = 11 + k + indexRowStartTransactionList[i];
                    endRow = 11 + k + q + indexRowStartTransactionList[i];

                    ws.Cells[11 + k + q + indexRowStartTransactionList[i], 22].Value = pays.Name + " от " + pays.Date.Value.ToShortDateString();
                    ws.Cells[11 + k + q + indexRowStartTransactionList[i], 29].Value = period.ToString("MMM yy");
                    ws.Cells[11 + k + q + indexRowStartTransactionList[i], 32].Value = pays.Total;
                    if (q == transactionsPayment.Count - 1) ws.Cells[3 + indexRowStartTransactionList[i], 26].Value = pays.Total;
                    q++;
                }
                if (firstRow != 0)
                {
                    ws.Cells[firstRow - 1, 32].Formula = "SUM(AF" + firstRow + ":AF" + endRow + ")";
                    firstRow = endRow = 0;
                }
                //else ws.Cells[firstRow - 1, 32].Value = 0;

                //заполнение долга на начало месяца
                var balance = Balance(period, currentWorker.Id);
                if (transactionsPayment.Count != 0) --q;
                ws.Cells[13 + k + q + indexRowStartTransactionList[i], 18].Value = balance;
                var balanceEnd = Balance(period.AddMonths(1), currentWorker.Id);
                ws.Cells[13 + k + q + indexRowStartTransactionList[i], 32].Value = balanceEnd;

                if (i < workersId.Length - 1)
                {
                    indexRowStartTransactionList[i + 1] = indexRowStartTransactionList[i] + totalRows + insertedRows;
                    insertedRows = 0;
                }
            }

            //сохранение и открытие файла
            var bin = pck.GetAsByteArray();
            File.WriteAllBytes(@"Шаблоны/Расчетный-лист-" + Environment.MachineName + ".xlsx", bin);
            var file = new FileInfo(@"Шаблоны/Расчетный-лист-" + Environment.MachineName + ".xlsx");
            Process.Start(file.FullName);
        }


        public decimal Balance(DateTime date, int? worker)
        {
            var dateStart = new DateTime(date.Year, date.Month, 1);
            //определение периода начислений
            //определяем дату первого начисления
            DateTime dateBegin;
            decimal profit, retention, payment;
            using (IstraContext db = new IstraContext())
            {
                //определение даты начала операций
                if (worker == null)
                    dateBegin = db.Retentions.OrderBy(a => a.Month).Select(a => a.Month).FirstOrDefault();
                else
                    dateBegin = db.Retentions.Where(a => a.WorkerId == worker).OrderBy(a => a.Month).Select(a => a.Month).FirstOrDefault();

                //получение суммы начислений за период до начала отчетного                
                if (worker == null)
                    profit = db.Retentions.Include(a => a.TypeOfTransaction).Where(a => a.Month >= dateBegin
                    && a.Month < dateStart && a.TypeOfTransaction.Name == "Начисление").Select(a => a.Value).DefaultIfEmpty(0).Sum();
                else
                    profit = db.Retentions.Include(a => a.TypeOfTransaction).Where(a => a.Month >= dateBegin
                    && a.Month < dateStart && a.WorkerId == worker && a.TypeOfTransaction.Name == "Начисление").Select(a => a.Value).DefaultIfEmpty(0).Sum();

                //получение суммы удержаний за период до начала отчетного                
                if (worker == null)
                    retention = db.Retentions.Include(a => a.TypeOfTransaction).Where(a => a.Month >= dateBegin
                    && a.Month < dateStart && a.TypeOfTransaction.Name == "Удержание").Select(a => a.Value).DefaultIfEmpty(0).Sum();
                else
                    retention = db.Retentions.Include(a => a.TypeOfTransaction).Where(a => a.Month >= dateBegin && a.Month < dateStart
                    && a.WorkerId == worker && a.TypeOfTransaction.Name == "Удержание").Select(a => a.Value).DefaultIfEmpty(0).Sum();

                //получение суммы выплат за период до начала отчетного                
                if (worker == null)
                    payment = db.Retentions.Include(a => a.TypeOfTransaction).Where(a => a.Month >= dateBegin
                    && a.Month < dateStart && a.TypeOfTransaction.Name == "Выплата").Select(a => a.Value).DefaultIfEmpty(0).Sum();
                else
                    payment = db.Retentions.Include(a => a.TypeOfTransaction).Where(a => a.Month >= dateBegin && a.Month < dateStart
                    && a.WorkerId == worker && a.TypeOfTransaction.Name == "Выплата").Select(a => a.Value).DefaultIfEmpty(0).Sum();
            }
            return profit - retention - payment;
        }

        public void ExportJournal(DataGridView dgv)
        {
            try
            {
                using (ExcelPackage eP = new ExcelPackage())
                {
                    eP.Workbook.Properties.Author = "ИСТРА";
                    eP.Workbook.Properties.Title = "Список учащихся";
                    eP.Workbook.Properties.Company = "ИСТРА";

                    var sheet = eP.Workbook.Worksheets.Add("Список учащихся");

                    var row = 1;
                    var col = 0;

                    // шапка                
                    List<string> columnsVisible = new List<string>();
                    //создание столбцов шапки
                    foreach (DataGridViewColumn column in dgv.Columns)
                    {
                        if (column.Visible != false)
                        {
                            columnsVisible.Add(column.Name);
                            sheet.Cells[row, ++col].Value = column.HeaderText.Replace("\r\n", ".");
                        }
                    }
                    ////оформление шапки
                    //using (var cells = sheet.Cells[sheet.Dimension.Address])
                    //{
                    //    //cells.Style.Font.Bold = true;
                    //    cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    //}
                    row++;
                    col = 1;
                    //заполнение таблицы данными
                    foreach (String c in columnsVisible)
                    {
                        if (c == "Students")
                        {
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                            }
                            sheet.Column(col).Width = 34;
                            sheet.Cells[1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet.Cells[1, col].Style.Font.Bold = true;
                        }
                        else if (c == "Number")
                        {
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                            }
                            sheet.Column(col).Width = 4;
                            sheet.Cells[1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet.Cells[1, col].Style.Font.Bold = true;
                        }
                        else
                        {
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                int sum;
                                if (!string.IsNullOrEmpty(dgv.Rows[a].Cells[c].Value.ToString()))
                                {
                                    if (dgv.Rows[a].Cells[c].Value.ToString().ToLower().Trim() != "н"
                                        && dgv.Rows[a].Cells[c].Value.ToString().ToLower().Trim() != "с"
                                        && dgv.Rows[a].Cells[c].Value.ToString().ToLower().Trim() != "п"
                                        && dgv.Rows[a].Cells[c].Value.ToString().ToLower().Trim() != "о")
                                    {
                                        sum = (string.IsNullOrEmpty(dgv.Rows[a].Cells[c].Value.ToString())) ? 0 : Convert.ToInt32(dgv.Rows[a].Cells[c].Value);
                                        sheet.Cells[a + row, col].Value = sum;
                                        sheet.Cells[a + row, col].Style.Numberformat.Format = "#";
                                    }
                                    else
                                    {
                                        sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value.ToString();
                                    }
                                    sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                }
                            }
                            sheet.Column(col).Width = 6;
                            sheet.Cells[1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            sheet.Cells[1, col].Style.Font.Bold = true;
                        }
                        col++;
                    }

                    // сохраняем в файл
                    Directory.CreateDirectory($"Отчеты");
                    var bin = eP.GetAsByteArray();
                    try
                    {
                        File.WriteAllBytes(@"Отчеты/Журнал-" + DateTime.Now.ToShortDateString() + ".xlsx", bin);
                        var file = new FileInfo(@"Отчеты/Журнал-" + DateTime.Now.ToShortDateString() + ".xlsx");
                        Process.Start(file.FullName);
                    }
                    catch (IOException ex)
                    {
                        CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                        MessageBox.Show("Файл со списком слушателей не может быть создан. Если файл уже создавался сегодня и открыт, закройте его и повторите попытку.",
                            "Ошибка создания файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                        MessageBox.Show($"При открытии файла произошла ошибка:\n{ex.Message}", "Ошибка открытия файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ExportExcelPayments(DataGridView dgv)
        {
            try
            {
                using (ExcelPackage eP = new ExcelPackage())
                {
                    eP.Workbook.Properties.Author = "ИСТРА";
                    eP.Workbook.Properties.Title = "Список учащихся";
                    eP.Workbook.Properties.Company = "ИСТРА";

                    var sheet = eP.Workbook.Worksheets.Add("Список платежей");

                    var row = 1;
                    var col = 1;

                    // шапка                
                    List<string> columnsVisible = new List<string>();
                    columnsVisible.Add("№");
                    //создание столбца с нумерацией
                    sheet.Cells[row, col].Value = "№";
                    //создание остальных столбцов шапки
                    foreach (DataGridViewColumn column in dgv.Columns)
                    {
                        if (column.Visible != false && column.Name != "IsSelected")
                        {
                            columnsVisible.Add(column.Name);
                            sheet.Cells[row, ++col].Value = column.HeaderText;
                        }
                    }
                    //оформление шапки
                    using (var cells = sheet.Cells[sheet.Dimension.Address])
                    {
                        cells.Style.Font.Bold = true;
                        cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    row++;
                    col = 1;
                    //заполнение таблицы данными
                    foreach (String c in columnsVisible)
                    {
                        if (c == "№")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = a + 1;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c == "GroupName" || c == "Type")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c.IndexOf("name") > -1 || c == "Note")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                        else if (c.IndexOf("ValuePayment") > -1)
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                double sum = Convert.ToDouble(dgv.Rows[a].Cells[c].Value); //(c.GetValue(listGroupFull[a]).ToString() == "") ? 0 : Convert.ToDouble(c.GetValue(listGroupFull[a]));
                                sheet.Cells[a + row, col].Style.Numberformat.Format = "0.00";
                                sheet.Cells[a + row, col].Value = sum;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                        else if (c.IndexOf("Date") > -1)
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Style.Numberformat.Format = "dd/MM/yyyy";
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        col++;
                    }
                    //оформление данных
                    using (var cells = sheet.Cells[sheet.Dimension.Address])
                    {
                        cells.AutoFitColumns();
                    }

                    // сохраняем в файл
                    Directory.CreateDirectory($"Отчеты");
                    var bin = eP.GetAsByteArray();
                    try
                    {
                        File.WriteAllBytes(@"Отчеты/Список слушателей-" + DateTime.Now.ToShortDateString() + ".xlsx", bin);
                        var file = new FileInfo(@"Отчеты/Список слушателей-" + DateTime.Now.ToShortDateString() + ".xlsx");
                        Process.Start(file.FullName);
                    }
                    catch (IOException ex)
                    {
                        CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                        MessageBox.Show("Файл со списком слушателей не может быть создан. Если файл уже создавался сегодня и открыт, закройте его и повторите попытку.",
                            "Ошибка создания файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                        MessageBox.Show($"При открытии файла произошла ошибка:\n{ex.Message}", "Ошибка открытия файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void ExportExcelLessons(DataGridView dgv, string role)
        {
            try
            {
                using (ExcelPackage eP = new ExcelPackage())
                {
                    eP.Workbook.Properties.Author = "ИСТРА";
                    eP.Workbook.Properties.Title = "Список занятий";
                    eP.Workbook.Properties.Company = "ИСТРА";

                    var sheet = eP.Workbook.Worksheets.Add("Список занятий");

                    var row = 1;
                    var col = 1;

                    // шапка                
                    List<string> columnsVisible = new List<string>();
                    columnsVisible.Add("№");
                    //создание столбца с нумерацией
                    sheet.Cells[row, col].Value = "№";
                    //создание остальных столбцов шапки
                    foreach (DataGridViewColumn column in dgv.Columns)
                    {
                        if (column.Visible != false)
                        {
                            columnsVisible.Add(column.Name);
                            sheet.Cells[row, ++col].Value = column.HeaderText;
                        }
                    }

                    //оформление шапки
                    using (var cells = sheet.Cells[sheet.Dimension.Address])
                    {
                        cells.Style.Font.Bold = true;
                        cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    row++;
                    col = 1;
                    //заполнение таблицы данными
                    foreach (String c in columnsVisible)
                    {
                        if (c == "№")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = a + 1;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c == "Number" || c == "Students" || c == "DurationLesson")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c == "Teacher" || c == "GroupName" || c == "CourseName" || c == "DirectionName" || c == "Branch" || c == "Class" || c == "Topic")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                        else if (c.IndexOf("DateLesson") > -1)
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Style.Numberformat.Format = "dd/MM/yyyy";
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        else if (c == "Wage" || c == "Wages")
                            for (int a = 0; a < dgv.Rows.Count; a++)
                            {
                                sheet.Cells[a + row, col].Value = dgv.Rows[a].Cells[c].Value;
                                sheet.Cells[a + row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                        col++;
                    }
                    //оформление данных
                    using (var cells = sheet.Cells[sheet.Dimension.Address])
                    {
                        cells.AutoFitColumns();
                    }

                    //добавление листа со сводной
                    var sheet1 = eP.Workbook.Worksheets.Add("Сводная");

                    List<ListLessons> lst = dgv.DataSource as List<ListLessons>;
                    var summary = from l in lst
                                  group l by new { l.Teacher, l.GroupName, l.Students, l.Wage, l.Branch } into g
                                  orderby g.Key.Teacher, g.Key.GroupName
                                  select new
                                  {
                                      g.Key.Teacher,
                                      Group = g.Key.GroupName,
                                      g.Key.Students,
                                      Hours = g.Sum(a => a.DurationLesson),
                                      g.Key.Wage,
                                      Housing = g.Key.Branch
                                  };
                    int ind = 0; int delta = 0;
                    string teach = "";
                    int countHousings = 0;
                    int indexTitleBranch = 0;
                    foreach (var s in summary)
                    {
                        if (teach != s.Teacher)
                        {
                            //высчитывание итогов
                            if (teach != "" && ind != 0)
                            {
                                sheet1.Cells[ind, 1].Value = "Итого";
                                sheet1.Cells[ind, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet1.Cells[ind, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet1.Cells[ind, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                sheet1.Cells[ind, 1].Style.Font.Bold = true;

                                sheet1.Cells[ind, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet1.Cells[ind, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet1.Cells["B" + ind].Formula = "SUM(B" + delta + ":B" + (ind - 1) + ")";
                                sheet1.Cells[ind, 2].Style.Font.Bold = true;

                                sheet1.Cells[ind, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet1.Cells[ind, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet1.Cells[ind, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet1.Cells["C" + ind].Formula = "SUM(C" + delta + ":C" + (ind - 1) + ")";
                                sheet1.Cells[ind, 3].Style.Font.Bold = true;

                                sheet1.Cells[ind, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                sheet1.Cells[ind, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet1.Cells[ind, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                sheet1.Cells[ind, 5].Formula = "SUM(E" + delta + ":E" + (ind - 1) + ")";
                                sheet1.Cells[ind, 5].Style.Font.Bold = true;
                                sheet1.Cells[ind, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                                for (int i = 0; i < countHousings; i++)
                                {
                                    char c = (char)(i + 7 + 64);
                                    sheet1.Cells[ind, i + 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    sheet1.Cells[ind, i + 7].Formula = "SUM(" + c + delta + ":" + c + (ind - 1) + ")";
                                    sheet1.Cells[ind, i + 7].Style.Font.Bold = true;
                                    sheet1.Cells[ind, i + 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                                }

                                ind += 2;
                            }

                            ind++;
                            sheet1.Cells[ind, 1].Value = s.Teacher;
                            sheet1.Cells[ind, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            sheet1.Cells[ind, 1].Style.Font.Bold = true;
                            sheet1.Cells[ind, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            //Color yellow = Color.FromArgb(255, 255, 0);
                            sheet1.Cells[ind, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;                            
                            sheet1.Cells[ind, 1].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                            sheet1.Column(1).Width = 18;
                            ind++;
                            teach = s.Teacher;
                            sheet1.Cells[ind, 1].Value = "Группа";
                            sheet1.Cells[ind, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet1.Cells[ind, 1].Style.Font.Bold = true;
                            sheet1.Cells[ind, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            sheet1.Cells[ind, 2].Value = "Кол-во";
                            sheet1.Cells[ind, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet1.Cells[ind, 2].Style.Font.Bold = true;
                            sheet1.Cells[ind, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            sheet1.Cells[ind, 3].Value = "Часов";
                            sheet1.Cells[ind, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet1.Cells[ind, 3].Style.Font.Bold = true;
                            sheet1.Cells[ind, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            sheet1.Cells[ind, 4].Value = "Ставка";
                            sheet1.Cells[ind, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet1.Cells[ind, 4].Style.Font.Bold = true;
                            sheet1.Cells[ind, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            sheet1.Cells[ind, 5].Value = "Сумма";
                            sheet1.Cells[ind, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            sheet1.Cells[ind, 5].Style.Font.Bold = true;
                            sheet1.Cells[ind, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            sheet1.Cells[ind, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                            //добавление корпусов в отчет
                            var housings = lst.Select(a => a.Branch).Distinct().ToList();
                            countHousings = housings.Count();
                            indexTitleBranch = ind;
                            for (int i = 0; i < countHousings; i++)
                            {
                                sheet1.Cells[ind, i + 7].Value = housings[i];
                                sheet1.Cells[ind, i + 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet1.Cells[ind, i + 7].Style.Font.Bold = true;
                                sheet1.Cells[ind, i + 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            }

                            delta = ++ind;//начальная строка для подсчета итогов
                        }

                        sheet1.Cells[ind, 1].Value = s.Group;
                        sheet1.Cells[ind, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        sheet1.Cells[ind, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet1.Cells[ind, 2].Value = s.Students;
                        sheet1.Cells[ind, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet1.Cells[ind, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet1.Cells[ind, 3].Value = s.Hours;
                        sheet1.Cells[ind, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet1.Cells[ind, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet1.Cells[ind, 4].Value = s.Wage;
                        sheet1.Cells[ind, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet1.Cells[ind, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        sheet1.Cells[ind, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        sheet1.Cells["E" + ind].Formula = "=C" + ind + "*D" + ind;
                        sheet1.Cells[ind, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        sheet1.Cells[ind, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                        for (int i = 0; i < countHousings; i++)
                        {
                            if (sheet1.Cells[indexTitleBranch, i + 7].Value.ToString() == s.Housing)
                            {
                                sheet1.Cells[ind, i + 7].Value = s.Hours;
                                sheet1.Cells[ind, i + 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                sheet1.Cells[ind, i + 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            }
                        }
                        ind++;
                    }
                    sheet1.Cells[ind, 1].Value = "Итого";
                    sheet1.Cells[ind, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    sheet1.Cells[ind, 1].Style.Font.Bold = true;
                    sheet1.Cells[ind, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet1.Cells[ind, 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet1.Cells[ind, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet1.Cells["C" + ind].Formula = "SUM(C" + delta + ":C" + (ind - 1) + ")";
                    sheet1.Cells[ind, 3].Style.Font.Bold = true;
                    sheet1.Cells[ind, 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet1.Cells[ind, 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    sheet1.Cells[ind, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    sheet1.Cells[ind, 5].Formula = "SUM(E" + delta + ":E" + (ind - 1) + ")";
                    sheet1.Cells[ind, 5].Style.Font.Bold = true;
                    sheet1.Cells[ind, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    sheet1.Cells[ind, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    for (int i = 0; i < countHousings; i++)
                    {
                        char c = (char)(i + 7 + 64);
                        sheet1.Cells[ind, i + 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        sheet1.Cells[ind, i + 7].Formula = "SUM(" + c + delta + ":" + c + (ind - 1) + ")";
                        sheet1.Cells[ind, i + 7].Style.Font.Bold = true;
                        sheet1.Cells[ind, i + 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }

                    // сохраняем в файл
                    Directory.CreateDirectory($"Отчеты");
                    var bin = eP.GetAsByteArray();
                    try
                    {
                        File.WriteAllBytes(@"Отчеты/Список занятий-" + DateTime.Now.ToShortDateString() + ".xlsx", bin);
                        var file = new FileInfo(@"Отчеты/Список занятий-" + DateTime.Now.ToShortDateString() + ".xlsx");
                        Process.Start(file.FullName);
                    }
                    catch (IOException ex)
                    {
                        CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                        MessageBox.Show("Файл со списком занятий не может быть создан. Если файл уже создавался сегодня и открыт, закройте его и повторите попытку.",
                            "Ошибка создания файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                        MessageBox.Show($"При открытии файла произошла ошибка:\n{ex.Message}", "Ошибка открытия файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                CurrentSession.ReportError(new StackTrace(false).GetFrame(0).GetMethod().Name, ex.Message);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

