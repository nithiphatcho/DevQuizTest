using Microsoft.AspNetCore.Mvc;

namespace DevQuizTest.Controllers
{
    public class DevQuizController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CalculateDiscount(Dictionary<string, int> books)
        {
            const decimal bookPrice = 100m;
            decimal totalPrice = 0;
            decimal totalDiscount = 0;

            // คำนวณจำนวนหนังสือทั้งหมด
            int totalBooks = books.Values.Sum();

            // สร้างรายการนับจำนวนหนังสือที่ไม่ซ้ำกัน
            var distinctBooks = books.Where(b => b.Value > 0).ToList();

            // จัดกลุ่มหนังสือที่ไม่ซ้ำในแต่ละรอบ
            while (distinctBooks.Any(b => b.Value > 0))
            {
                // เลือก 1 เล่มจากแต่ละรายการที่ยังเหลืออยู่
                int booksInGroup = distinctBooks.Count(b => b.Value > 0);
                decimal discountPercentage = 0;

                // กำหนดส่วนลดตามจำนวนหนังสือในกลุ่ม
                switch (booksInGroup)
                {
                    case 2:
                        discountPercentage = 10;
                        break;
                    case 3:
                        discountPercentage = 20;
                        break;
                    case 4:
                        discountPercentage = 30;
                        break;
                    case 5:
                        discountPercentage = 40;
                        break;
                    case 6:
                        discountPercentage = 50;
                        break;
                    case 7:
                        discountPercentage = 60;
                        break;
                }

                // คำนวณราคาสำหรับกลุ่มนี้
                decimal groupPrice = booksInGroup * bookPrice;
                decimal groupDiscount = (discountPercentage / 100) * groupPrice;

                totalPrice += groupPrice;
                totalDiscount += groupDiscount;

                // ลดจำนวนหนังสือในแต่ละรายการที่ใช้ไป
                for (int i = 0; i < distinctBooks.Count; i++)
                {
                    if (distinctBooks[i].Value > 0)
                    {
                        distinctBooks[i] = new KeyValuePair<string, int>(
                            distinctBooks[i].Key,
                            distinctBooks[i].Value - 1
                        );
                    }
                }
            }

            decimal finalPrice = totalPrice - totalDiscount;

            // ส่งผลลัพธ์ไปที่ View
            ViewBag.TotalBooks = totalBooks;
            ViewBag.TotalPrice = totalPrice;
            ViewBag.TotalDiscount = totalDiscount;
            ViewBag.FinalPrice = finalPrice;

            return View("Index");
        }
    }
}

