using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity; // Ensure you have the Entity Framework package installed
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MVVMDemo
{
    //public class CustomerViewModel : IDisposable
    public class CustomerViewModel : INotifyPropertyChanged
    {
        private NorthwindEntities _db;
        private bool _disposed;
        public List<Customer> Customers { get; private set; }

        /// <summary>
        /// setCustomers プロパティは、顧客のリストを表します。
        /// </summary>
        public ObservableCollection<Customer> CustomerList { get; } = new ObservableCollection<Customer>();
        private Customer _selectedCustomer;
        /// <summary>
        /// selectedCustomer プロパティは、選択された顧客を表します。
        /// </summary>
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }

   


        // 修正: LoadCustomersAsync メソッドのアクセス修飾子を 'private' から 'public' に変更  
        public async Task LoadCustomersAsync()
        {
            _db = new NorthwindEntities();
            Customers = await Task.Run(() => _db.Customers.ToList());
            INotifyPropertyChanged(nameof(Customers));
        }

        public void Dispose() { /* 既存のコード */ }
        public void DeleteCustomer(Customer customer) { /* 既存のコード */ }
        public void NewCustomer(Customer customer) 
        {
            try
            {
                _db.Customers.Add(customer);
                Customers.Add(customer);
                //SaveChanges();
                //OnPropertyChanged(nameof(Customers)); // メソッド名を変更
            }
            catch (Exception ex)
            {
                // ログ出力やエラーメッセージ表示など
            }
        }
        public void UpdateCustomer(Customer customer) { /* 既存のコード */ }
        public void SaveChanges()
        {
            if (_db != null)
            {
                _db.SaveChanges();
                OnPropertyChanged(nameof(Customers));
            }
        }
        protected virtual void Dispose(bool disposing) { /* 既存のコード */ }


        public void INotifyPropertyChanged(string propertyName) 
        {
            
            
            /* 既存のコード */ }



        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }      
}
