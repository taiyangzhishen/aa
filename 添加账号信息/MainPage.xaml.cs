using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace 添加账号信息
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static SQLiteAsyncConnection db { get; set; }
        ObservableCollection<word> list = new ObservableCollection<word>();
        public MainPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (e.NewSize.Width < 1000)
                {
                    state = "VisualState1000";
                }
                VisualStateManager.GoToState(this, state, true);
            };
        }

        public async void add(string t, string p)
        {
            db = new SQLiteAsyncConnection("Word.db");
            await db.CreateTableAsync<word>();
            word newWord = new word { Title = title.Text, _passage = newPassage.Text};
            await db.InsertAsync(newWord);
            listView.ItemsSource = list;
        }

        public async void Delete()
        {
            db = new SQLiteAsyncConnection("Word.db");
            await db.CreateTableAsync<word>();
            var query = await (db.Table<word>().Where(v => v.Title.Equals(title))).ToListAsync();
            await db.DeleteAsync(query[0]);
        }

        private async void save_Click(object sender, RoutedEventArgs e)
        {
            if(title.Text == null)
            {
                ContentDialog noTitleDialog = new ContentDialog()
                {
                    Title = "标题为空",
                    Content = "请重新输入标题",
                    PrimaryButtonText = "确定"
                };

                ContentDialogResult result = await noTitleDialog.ShowAsync();
            }
            else
            {
                if(newPassage.Text == null)
                {
                    ContentDialog noPassageDialog = new ContentDialog()
                    {
                        Title = "正文为空",
                        Content = "请重新输入文章内容",
                        PrimaryButtonText = "确定"
                    };
                    ContentDialogResult result = await noPassageDialog.ShowAsync();
                }
                else
                {
                    add(title.Text, newPassage.Text);
                }
            }
        }

        private void listView_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        private async void delete_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog noCantDeleDialog = new ContentDialog()
            {
                Title = "无法删除",
                Content = "请不要重试，谢谢",
                PrimaryButtonText = "确定"
            };
            ContentDialogResult result = await noCantDeleDialog.ShowAsync();
        }
    }
}

public class word
{
    [PrimaryKey, AutoIncrement]
    public string Title { get; set; }
    public string _passage { get; set; }
} 
