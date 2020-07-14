namespace Istra
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.учащиесяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьСлушателяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.списокСлушателейToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.архивToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.группыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьГруппуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.списокГруппToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.списокЗанятийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.платежиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.платежиToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.возвратПлатежейToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.планПриемаВГруппыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отчетУчащиесяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отчетГруппыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.заработнаяПлатаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.чтоНовогоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.учащиесяToolStripMenuItem,
            this.группыToolStripMenuItem,
            this.платежиToolStripMenuItem,
            this.настройкаToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1325, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // учащиесяToolStripMenuItem
            // 
            this.учащиесяToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьСлушателяToolStripMenuItem,
            this.списокСлушателейToolStripMenuItem,
            this.архивToolStripMenuItem});
            this.учащиесяToolStripMenuItem.Name = "учащиесяToolStripMenuItem";
            this.учащиесяToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.учащиесяToolStripMenuItem.Text = "Учащиеся";
            // 
            // добавитьСлушателяToolStripMenuItem
            // 
            this.добавитьСлушателяToolStripMenuItem.Name = "добавитьСлушателяToolStripMenuItem";
            this.добавитьСлушателяToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.добавитьСлушателяToolStripMenuItem.Text = "Добавить учащегося";
            this.добавитьСлушателяToolStripMenuItem.Click += new System.EventHandler(this.добавитьСлушателяToolStripMenuItem_Click);
            // 
            // списокСлушателейToolStripMenuItem
            // 
            this.списокСлушателейToolStripMenuItem.Name = "списокСлушателейToolStripMenuItem";
            this.списокСлушателейToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.списокСлушателейToolStripMenuItem.Text = "Список учащихся";
            this.списокСлушателейToolStripMenuItem.Click += new System.EventHandler(this.списокСлушателейToolStripMenuItem_Click);
            // 
            // архивToolStripMenuItem
            // 
            this.архивToolStripMenuItem.Name = "архивToolStripMenuItem";
            this.архивToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.архивToolStripMenuItem.Text = "Архив";
            this.архивToolStripMenuItem.Click += new System.EventHandler(this.архивToolStripMenuItem_Click);
            // 
            // группыToolStripMenuItem
            // 
            this.группыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьГруппуToolStripMenuItem,
            this.списокГруппToolStripMenuItem,
            this.списокЗанятийToolStripMenuItem});
            this.группыToolStripMenuItem.Name = "группыToolStripMenuItem";
            this.группыToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.группыToolStripMenuItem.Text = "Группы";
            // 
            // добавитьГруппуToolStripMenuItem
            // 
            this.добавитьГруппуToolStripMenuItem.Name = "добавитьГруппуToolStripMenuItem";
            this.добавитьГруппуToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.добавитьГруппуToolStripMenuItem.Text = "Добавить группу";
            this.добавитьГруппуToolStripMenuItem.Click += new System.EventHandler(this.добавитьГруппуToolStripMenuItem_Click);
            // 
            // списокГруппToolStripMenuItem
            // 
            this.списокГруппToolStripMenuItem.Name = "списокГруппToolStripMenuItem";
            this.списокГруппToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.списокГруппToolStripMenuItem.Text = "Список групп";
            this.списокГруппToolStripMenuItem.Click += new System.EventHandler(this.списокГруппToolStripMenuItem_Click);
            // 
            // списокЗанятийToolStripMenuItem
            // 
            this.списокЗанятийToolStripMenuItem.Name = "списокЗанятийToolStripMenuItem";
            this.списокЗанятийToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.списокЗанятийToolStripMenuItem.Text = "Список занятий";
            this.списокЗанятийToolStripMenuItem.Click += new System.EventHandler(this.списокЗанятийToolStripMenuItem_Click);
            // 
            // платежиToolStripMenuItem
            // 
            this.платежиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.платежиToolStripMenuItem1,
            this.возвратПлатежейToolStripMenuItem,
            this.планПриемаВГруппыToolStripMenuItem,
            this.отчетУчащиесяToolStripMenuItem,
            this.отчетГруппыToolStripMenuItem,
            this.заработнаяПлатаToolStripMenuItem});
            this.платежиToolStripMenuItem.Name = "платежиToolStripMenuItem";
            this.платежиToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.платежиToolStripMenuItem.Text = "Финансы";
            // 
            // платежиToolStripMenuItem1
            // 
            this.платежиToolStripMenuItem1.Name = "платежиToolStripMenuItem1";
            this.платежиToolStripMenuItem1.Size = new System.Drawing.Size(201, 22);
            this.платежиToolStripMenuItem1.Text = "Список платежей";
            this.платежиToolStripMenuItem1.Click += new System.EventHandler(this.платежиToolStripMenuItem1_Click);
            // 
            // возвратПлатежейToolStripMenuItem
            // 
            this.возвратПлатежейToolStripMenuItem.Name = "возвратПлатежейToolStripMenuItem";
            this.возвратПлатежейToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.возвратПлатежейToolStripMenuItem.Text = "Возврат платежей";
            this.возвратПлатежейToolStripMenuItem.Click += new System.EventHandler(this.возвратПлатежейToolStripMenuItem_Click);
            // 
            // планПриемаВГруппыToolStripMenuItem
            // 
            this.планПриемаВГруппыToolStripMenuItem.Name = "планПриемаВГруппыToolStripMenuItem";
            this.планПриемаВГруппыToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.планПриемаВГруппыToolStripMenuItem.Text = "План приема в группы";
            this.планПриемаВГруппыToolStripMenuItem.Click += new System.EventHandler(this.планПриемаВГруппыToolStripMenuItem_Click);
            // 
            // отчетУчащиесяToolStripMenuItem
            // 
            this.отчетУчащиесяToolStripMenuItem.Name = "отчетУчащиесяToolStripMenuItem";
            this.отчетУчащиесяToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.отчетУчащиесяToolStripMenuItem.Text = "Отчет по учащимся";
            this.отчетУчащиесяToolStripMenuItem.Click += new System.EventHandler(this.отчетыToolStripMenuItem_Click);
            // 
            // отчетГруппыToolStripMenuItem
            // 
            this.отчетГруппыToolStripMenuItem.Name = "отчетГруппыToolStripMenuItem";
            this.отчетГруппыToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.отчетГруппыToolStripMenuItem.Text = "Отчет по группам";
            this.отчетГруппыToolStripMenuItem.Click += new System.EventHandler(this.отчетГруппыToolStripMenuItem_Click);
            // 
            // заработнаяПлатаToolStripMenuItem
            // 
            this.заработнаяПлатаToolStripMenuItem.Name = "заработнаяПлатаToolStripMenuItem";
            this.заработнаяПлатаToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.заработнаяПлатаToolStripMenuItem.Text = "Заработная плата";
            this.заработнаяПлатаToolStripMenuItem.Click += new System.EventHandler(this.заработнаяПлатаToolStripMenuItem_Click);
            // 
            // настройкаToolStripMenuItem
            // 
            this.настройкаToolStripMenuItem.Name = "настройкаToolStripMenuItem";
            this.настройкаToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.настройкаToolStripMenuItem.Text = "Настройка";
            this.настройкаToolStripMenuItem.Click += new System.EventHandler(this.настройкаToolStripMenuItem_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.оПрограммеToolStripMenuItem,
            this.чтоНовогоToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // чтоНовогоToolStripMenuItem
            // 
            this.чтоНовогоToolStripMenuItem.Name = "чтоНовогоToolStripMenuItem";
            this.чтоНовогоToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.чтоНовогоToolStripMenuItem.Text = "Что нового?";
            this.чтоНовогоToolStripMenuItem.Click += new System.EventHandler(this.чтоНовогоToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1325, 634);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Образовательный центр \"ИСТРА\" ";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem учащиесяToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem группыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem списокСлушателейToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьСлушателяToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьГруппуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem списокГруппToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem платежиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem платежиToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem архивToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem чтоНовогоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отчетУчащиесяToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отчетГруппыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem возвратПлатежейToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem планПриемаВГруппыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem заработнаяПлатаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem списокЗанятийToolStripMenuItem;
    }
}

