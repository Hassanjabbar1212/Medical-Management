using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Medical.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Medical.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Medical.Controllers
{
    public class Item : Controller
    {
        private readonly Context _context;
        public Item(Context context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult List()
        {
         var items = _context.Items
      
        .Select(i => new ItemViewModel
     {
         Id = i.Id,
         MgId = i.Mgs != null ? i.Mgs.Id : (int?)null,
         MgName = i.Mgs != null ? i.Mgs.MgName : "",
         Name = i.Name,
         IsBox = true,
         Quantity = i.Quantity,
         BoxPrice = i.BoxPrice,
         SinglePrice = i.SinglePrice,
     })
     .ToList();
            return View(items);
        }
        
        public IActionResult Index()
        {
            var items = _context.Items
     .Where(i => i.Isbox == true)
     .Select(i => new ItemViewModel
     {
         Id = i.Id,
         MgId = i.Mgs != null ? i.Mgs.Id : (int?)null,
         MgName = i.Mgs != null ? i.Mgs.MgName : "",
         Name = i.Name,
         IsBox = true,
         Quantity = i.Quantity,
         BoxPrice = i.BoxPrice,
         SinglePrice = i.SinglePrice,
     })
     .ToList();
            return View(items);
        }
        [HttpGet]
        public IActionResult Single()
        {
            var items = _context.Items
    .Where(i => i.Isbox==false) 
    .Select(i => new ItemViewModel
    {
        Id = i.Id,
        MgId = i.Mgs != null ? i.Mgs.Id : (int?)null,
        MgName = i.Mgs != null ? i.Mgs.MgName : "",
        Name = i.Name,
        IsBox = false,
        Quantity = i.Quantity,
        BoxPrice = i.BoxPrice,
        SinglePrice = i.SinglePrice,
    })
    .ToList();
            return View(items);
        }
        [HttpGet]
        public IActionResult AddItem()
        {
            ViewBag.MgList = new SelectList(_context.mgs.ToList(), "Id", "MgName");
            return View();
        }
        [HttpPost]
        public IActionResult AddItem(Items item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]

        public IActionResult EditItem(int? id)
        {
            var content = _context.Items
    .Include(i => i.Mgs)
    .FirstOrDefault(a => a.Id == id);

            if (content == null)
            {
                return NotFound();
            }
            else
            {
                // Assuming Mg has a property named 'Name'
                var mgName = content.Mgs.MgName;

                // You can create a new model or use ViewBag to pass additional data to the view
                // For simplicity, let's assume you're using ViewBag
                ViewBag.MgName = mgName;

                return View(content);
            }
        }
        public IActionResult EditItems(Items updatedItems)
        {
            var existingItem = _context.Items.FirstOrDefault(i => i.Id == updatedItems.Id);

            if (existingItem == null)
            {
                return NotFound();
            }

            // Update properties of the existing item
            existingItem.Name = updatedItems.Name;
            existingItem.Mgs = updatedItems.Mgs;
            existingItem.SinglePrice = updatedItems.SinglePrice;
            existingItem.BoxPrice = updatedItems.BoxPrice;
            existingItem.Quantity = updatedItems.Quantity;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var content = _context.Items
    .Include(i => i.Mgs)
    .FirstOrDefault(a => a.Id == id);

            if (content == null)
            {
                return NotFound();
            }
            else
            {
                // Assuming Mg has a property named 'Name'
                //var mgName = content.Mgs.MgName;

                //// You can create a new model or use ViewBag to pass additional data to the view
                //// For simplicity, let's assume you're using ViewBag
                //ViewBag.MgName = mgName;

                return View(content);
            }
        }
        public IActionResult DeleteItem(int id)
        {
            var itemToDelete = _context.Items.FirstOrDefault(i => i.Id == id);

            if (itemToDelete == null)
            {
                return NotFound();
            }

            _context.Items.Remove(itemToDelete);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]

        public IActionResult SalesLog()
        {

            var items = _context.Items.ToList();
            ViewBag.Items = new SelectList(items, "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult SalesLog(SalesLog salesLog)
        {
            if (ModelState.IsValid)
            {
                // Save the sales log entry to the database or any other storage mechanism
                var salesLogEntry = new SalesLog
                {
                    SaleDate = DateTime.Now,
                    ItemId = salesLog.ItemId,
                    QuantitySold = salesLog.QuantitySold,
                    IsFullBox = salesLog.IsFullBox,
                };

                _context.SalesLog.Add(salesLogEntry);
                _context.SaveChanges();
                // Update the item quantity in the Items table
                var item = _context.Items.FirstOrDefault(i => i.Id == salesLog.ItemId);
                if (item != null)
                {
                    // Optionally, you can also update the box quantity based on IsFullBox
                    if (salesLog.IsFullBox==true)
                    {
                        if(item.Isbox==true)
                        {
                            item.Quantity -= salesLog.QuantitySold;
                        } 
                    }
                    else
                    {
                        item.Quantity -= salesLog.QuantitySold;
                    }

                    _context.SaveChanges();
                }

                // Redirect to the sales log page or any other page
                return RedirectToAction("SalesLog");
            }

            // If the model state is not valid, return to the same view with validation errors
            return View();
        }
        [HttpGet]
        public IActionResult SoldItems()
        {
            var soldItems = _context.SalesLog
                .Select(i => new SoldViewModel
                {
                   ItemName = i.Items.Name,
                   QuantitySold = i.QuantitySold,
                   IsFullBox = i.IsFullBox,
                   SaleDate = i.SaleDate,
                })
     .ToList();

                
            return View(soldItems);
        }
        [HttpGet]

        public IActionResult AddMG()
        {
            return View();
        }
        public IActionResult AddMG(Mg mg)
        {
            _context.mgs.Add(mg);
            _context.SaveChanges();
            return View();
        }
    }
}
