using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel; // ObservableCollection を使用するために必要

namespace MVVMDemo
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        private NorthwindEntities _db;
        private bool _disposed;

        // Customers を ObservableCollection に変更
        public ObservableCollection<Customer> Customers { get; private set; }

        private Customer _selectedCustomer;
        /// <summary>
        /// selectedCustomer プロパティは、選択された顧客を表します。
        /// </summary>
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (_selectedCustomer != value) // 値が変わった場合のみ更新
                {
                    _selectedCustomer = value;
                    OnPropertyChanged(nameof(SelectedCustomer));
                }
            }
        }

        public CustomerViewModel()
        {
            // ObservableCollection を初期化
            Customers = new ObservableCollection<Customer>();
            _db = new NorthwindEntities(); // コンストラクタでDBコンテキストを初期化
        }

        public async Task LoadCustomersAsync()
        {
            // 既存のデータをクリア
            Customers.Clear();
            // データベースからデータをロードし、ObservableCollection に追加
            var loadedCustomers = await Task.Run(() => _db.Customers.ToList());
            foreach (var customer in loadedCustomers)
            {
                Customers.Add(customer);
            }
            // Customers コレクション自体が変更されたことを通知
            OnPropertyChanged(nameof(Customers));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db?.Dispose();
                }
                _disposed = true;
            }
        }

        public void DeleteCustomer(Customer customer)
        {
            if (customer != null)
            {
                _db.Customers.Remove(customer);
                Customers.Remove(customer); // ObservableCollectionからも削除
                SaveChanges();
            }
        }

        public void NewCustomer(Customer customer)
        {
            try
            {
                _db.Customers.Add(customer);
                Customers.Add(customer); // ObservableCollectionに追加
                // SaveChanges(); は NewCustomer の後、必要に応じて呼び出す
                OnPropertyChanged(nameof(Customers)); // UIに更新を通知
            }
            catch (Exception ex)
            {
                // ログ出力やエラーメッセージ表示など
                System.Diagnostics.Debug.WriteLine($"Error adding new customer: {ex.Message}");
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            // EFがトラッキングしているので通常はSaveChange()で更新されるが、明示的な通知が必要な場合
            OnPropertyChanged(nameof(Customers)); // 例: コレクション全体の変更を通知
            // もしくは SelectedCustomer のプロパティ変更を通知 (必要に応じて)
            OnPropertyChanged(nameof(SelectedCustomer));
        }

        public void SaveChanges()
        {
            if (_db != null)
            {
                _db.SaveChanges();
                // SaveChanges後に、必要に応じてUIを更新するために通知
                OnPropertyChanged(nameof(Customers));
            }
        }

        // INotifyPropertyChanged メソッドの重複定義を修正 (不要な定義を削除)
        // public void INotifyPropertyChanged(string propertyName) { /* 既存のコード */ }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}