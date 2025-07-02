using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVVMDemo
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private CustomerViewModel vm;


        /// <summary>
        /// default コンストラクター
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            vm = new CustomerViewModel();
            this.DataContext = vm;

            // 非同期でデータをロードする
            await vm.LoadCustomersAsync();

            // ObservableCollectionを直接ItemsSourceにバインドする
            this.ClientDataGrid.ItemsSource = vm.Customers;

            System.Windows.Data.CollectionViewSource customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            // 必要に応じて、CollectionViewSourceを設定する
            customerViewSource.Source = vm.Customers;
            //新規ボタンの登録
            this.btnNew.Click += delegate
            {
               
                // 新しいCustomerオブジェクトを作成し、ViewModelに追加
                Customer newCustomer = new Customer();
                vm.NewCustomer(newCustomer);
                this.ClientDataGrid.SelectedItem = newCustomer;
            };
            //登録ボタンの登録
            this.btnSave.Click += delegate
            {
                vm.SaveChanges();
            };
        }


    }
}
