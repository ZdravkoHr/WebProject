using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBakeryFinal.Entities;
using MyBakeryFinal.Models;
using MyBakeryFinal.Repositories;
using MyBakeryFinal.ViewModels.Customers;
using System.Diagnostics;

namespace MyBakeryFinal.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ILogger<CustomersController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            IndexVM vm = new IndexVM();
            CustomersRepository customersRepo = new CustomersRepository();
            OrdersRepository ordersRepo = new OrdersRepository();
            List<Customer> allCustomers = customersRepo.GetAll(i => true);
            List<Order> allOrders = ordersRepo.GetAll(i => true);
            
            foreach (var customer in allCustomers)
            {
                int currentCount = 0;
                foreach(var order in allOrders)
                {
                    if (order.Customer_ID == customer.Id)
                    {
                        currentCount++;
                    }
                }

                customer.TotalOrders = currentCount;
            }

            vm.Customers = allCustomers;
            return View(vm);
        }

        [Authorize]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddVM addVM)
        {

            CustomersRepository customersRepo = new CustomersRepository();

            Customer item = new Customer();
            item.FirstName = addVM.FirstName;
            item.LastName = addVM.LastName;
            item.Address = addVM.Address;

            customersRepo.Save(item);


            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            CustomersRepository repo = new CustomersRepository();
            Customer customer = repo.GetAll(customer => customer.Id == id).Find(i => i.Id == id);

            EditVM vm = new EditVM();
            vm.Customer = customer;

            return View(vm);

        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(EditVM vm)
        {
            CustomersRepository repo = new CustomersRepository();

            repo.Save(vm.Customer);

            return RedirectToAction("Index");

        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            CustomersRepository repo = new CustomersRepository();

            Customer customer = new Customer();
            customer.Id = id;

            repo.Delete(customer);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}