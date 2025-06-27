using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.Entity; // Ensure you have the Entity Framework package installed

namespace MVVMDemo
{
    public class CustomerViewModel : IDisposable
    {
        private NorthwindEntities _db;
        private bool _disposed;
        public List<Customer> Customers { get; private set; }

        // 修正: LoadCustomersAsync メソッドのアクセス修飾子を 'private' から 'public' に変更  
        public async Task LoadCustomersAsync()
        {
            _db = new NorthwindEntities();
            Customers = await Task.Run(() => _db.Customers.ToList());
            INotifyPropertyChanged(nameof(Customers));
        }

        public void Dispose() { /* 既存のコード */ }
        public void DeleteCustomer(Customer customer) { /* 既存のコード */ }
        public void NewCustomer(Customer customer) { /* 既存のコード */ }
        public void UpdateCustomer(Customer customer) { /* 既存のコード */ }
        public void SaveChanges() { /* 既存のコード */ }
        protected virtual void Dispose(bool disposing) { /* 既存のコード */ }
        public void INotifyPropertyChanged(string propertyName) { /* 既存のコード */ }
        public event PropertyChangedEventHandler PropertyChanged;
    }      
}
