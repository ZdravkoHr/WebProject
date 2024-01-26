
using MyBakeryFinal.Models;
using MyBakeryFinal.ViewModels.Recipes;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MyBakeryFinal.ExtensionMethods;
using MyBakeryFinal.Entities;
using MyBakeryFinal.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace MyBakeryFinal.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ILogger<RecipesController> _logger;

        public RecipesController(ILogger<RecipesController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            IndexVM vm = new IndexVM();
            List<Baker> allBakers;

            BakersRepository bakersRepo = new BakersRepository();
            RecipesRepository recipesRepository = new RecipesRepository();
            RecipesToOrdersRepository recipesToOrdersRepository = new RecipesToOrdersRepository();
            List<Recipe> recipes = recipesRepository.GetAll(i => true);
            List<RecipesToOrders> allOrdersRelations = recipesToOrdersRepository.GetAll(item => true);

            Dictionary<int, int> recipesToCount = new Dictionary<int, int>();

			allBakers = bakersRepo.GetAll(i => true);

            foreach (var recipe in recipes)
            {
                Baker baker = allBakers.Find(b => b.Id == recipe.Baker_ID);
                recipe.Baker = baker;
            }

            foreach (var entry in allOrdersRelations)
            {
                if (recipesToCount.ContainsKey(entry.Recipe_ID))
                {
                    recipesToCount[entry.Recipe_ID]++;
                }
                else
                {
                    recipesToCount.Add(entry.Recipe_ID, 1);

				}
            }

            vm.Recipes = recipes;
            vm.RecipesToCount = recipesToCount;
            return View(vm);
        }

        public IActionResult Add()
        {
            AddVM vm = new AddVM();
            List<Baker> allBakers = new List<Baker>();

       
            BakersRepository bakersRepo = new BakersRepository();
            allBakers = bakersRepo.GetAll(i => true);
     
            vm.Bakers = allBakers;
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddVM addVM)
        {

            RecipesRepository recipesRepo = new RecipesRepository();

            Recipe item = new Recipe();
            item.Name = addVM.Name;
            item.Price = addVM.Price;
            item.Description = addVM.Description;
            item.Baker_ID = addVM.Baker_ID;

            recipesRepo.Save(item);


            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            RecipesRepository recipesRepo = new RecipesRepository();
			BakersRepository bakersRepo = new BakersRepository();

			Recipe recipe = recipesRepo.GetAll(recipe => recipe.Id == id).Find(i => i.Id == id);
            List<Baker> bakers = bakersRepo.GetAll(b => true).ToList();

            EditVM vm = new EditVM();
            vm.Recipe = recipe;
            vm.Bakers = bakers;

            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(EditVM vm)
        {
            RecipesRepository repo = new RecipesRepository();
            BakersRepository bakersRepo = new BakersRepository();

            Baker baker = bakersRepo.GetAll(baker => baker.Id == vm.Baker_ID).FirstOrDefault();
            Console.WriteLine(vm.Baker_ID);
            vm.Recipe.Baker = baker;

            repo.Save(vm.Recipe);

            return RedirectToAction("Index");

        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            RecipesRepository repo = new RecipesRepository();
            Recipe recipe = new Recipe();
            recipe.Id = id;

            repo.Delete(recipe);

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
