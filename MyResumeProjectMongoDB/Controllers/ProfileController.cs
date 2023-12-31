﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MyResumeProjectMongoDB.DAL.Entities;
using MyResumeProjectMongoDB.DAL.Settings;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyResumeProjectMongoDB.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IMongoCollection<Profile> _profileCollection;
        public ProfileController(IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _profileCollection = database.GetCollection<Profile>(_databaseSettings.ProfileCollectionName);

        }
        public async Task<IActionResult> Index()
        {
            var values = await _profileCollection.Find(x => true).ToListAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateProfile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile(Profile profile)
        {
            await _profileCollection.InsertOneAsync(profile);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteProfile(string id)
        {
            await _profileCollection.DeleteOneAsync(x => x.ProfileID == id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile(string id)
        {
            var values = await _profileCollection.Find(x => x.ProfileID == id).FirstOrDefaultAsync();
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(Profile profile)
        {
            await _profileCollection.FindOneAndReplaceAsync(x => x.ProfileID == profile.ProfileID, profile);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ProfileDetail(string id)
        {
			var values = await _profileCollection.Find(x => x.ProfileID == id).FirstOrDefaultAsync();
			return View(values);
		}

		[HttpPost]
		public async Task<IActionResult> ProfileDetail(Profile profile)
		{
			await _profileCollection.FindOneAndReplaceAsync(x => x.ProfileID == profile.ProfileID, profile);
			return RedirectToAction("Index");
		}
	}
}