namespace shopapp.entity
{
    public class OrderItem //sipariş edilen sepetteki ürünler
    {
        public int Id { get; set; }
        public int OrderId { get; set; }//order ver orderıtem arası 1-cok ilişko var
        public Order Order { get; set; }//order ver orderıtem arası 1-cok ilişko var
        public int ProductId { get; set; }//product ile orderıtem arası 1-çok ilişki var orderıtem daki ürün bilgilerine product tablosundan ulasmak isteriz ondan ekledik bunuda
        public Product Product { get; set; }//product ile orderıtem arası 1-çok ilişki var orderıtem daki ürün bilgilerine product tablosundan ulasmak isteriz ondan ekledik bunuda
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}