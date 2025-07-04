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
    public partial class MainWindow : Window // INotifyPropertyChanged は MainWindow で直接使用しないなら削除可能
    {
        // PropertyChangedイベントはViewModelで管理するため、MainWindowからは削除
        // public event PropertyChangedEventHandler PropertyChanged; 

        private CustomerViewModel vm;

        public MainWindow()
        {
            InitializeComponent(); // コンポーネントの初期化はここで行う
        }

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

            // ClientDataGrid.ItemsSource はXAMLでBindingするため、ここで設定は不要
            // this.ClientDataGrid.ItemsSource = vm.Customers; 

            // CollectionViewSource の設定も、直接バインディングする場合は通常不要
            // System.Windows.Data.CollectionViewSource customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            // customerViewSource.Source = vm.Customers; 

            //新規ボタンの登録
            this.btnNew.Click += delegate
            {
                Customer newCustomer = new Customer();
                vm.NewCustomer(newCustomer);
                this.ClientDataGrid.SelectedItem = newCustomer; // 新規追加後、選択状態にする
            };
            //登録ボタンの登録
            this.btnSave.Click += delegate
            {
                vm.SaveChanges();
            };

            // DataGridの選択変更イベントを購読し、SelectedCustomerをViewModelに反映させる
            // (XAMLでSelectedItem={Binding SelectedCustomer, Mode=TwoWay}と設定していれば不要)
            // this.ClientDataGrid.SelectionChanged += ClientDataGrid_SelectionChanged;
        }

        // DataGridの選択変更イベントハンドラ (XAMLでTwoWayバインディングしていれば不要)
        // private void ClientDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        // {
        //     if (ClientDataGrid.SelectedItem is Customer selectedCustomer)
        //     {
        //         vm.SelectedCustomer = selectedCustomer;
        //     }
        // }
    }
}