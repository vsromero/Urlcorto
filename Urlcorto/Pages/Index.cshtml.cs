using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Urlcorto.Models;

namespace Urlcorto.Pages
{
    public class IndexModel : PageModel
    {
        //private readonly Urlcorto.Data.UrlcortoContext _context;
        private readonly Urlcorto.Data.GCPContext _context;        
        //Set the base URL
        private readonly string BASEURL = "http://urlshortener-test1.us-east-1.elasticbeanstalk.com"; // <<--- Modify This

        public IndexModel(Urlcorto.Data.GCPContext context)
        {
            _context = context;
        }

        public Url MyUrl { get; set; }
        [BindProperty]
        [Required]
        [DataType(DataType.Url)]
        public string InputUrl { get; set; }

        public IActionResult OnGet(int? id, string u)
        {
            if (!string.IsNullOrEmpty(u))
            {
                var urlDecoded = Base64.Decode(u);
                var externalUrl = _context.Urls.FirstOrDefault(m => m.Id == urlDecoded).UrlToShorten;
                return Redirect(externalUrl);
            }

            if (id == null)
            {
                return Page();
            }

            MyUrl = _context.Urls.FirstOrDefault(m => m.Id == id);

            if (MyUrl == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (_context.Urls.Any(u => u.UrlToShorten.Equals(InputUrl)))
            {
                // if url to shorten already exists return shortened url in db
                var existingId = _context.Urls.FirstOrDefault(u => u.UrlToShorten.Equals(InputUrl)).Id;
                return RedirectToPage(new { id = existingId });
            }

            var newUrlRecord = new Url()
            {
                UrlToShorten = InputUrl
            };

            _context.Urls.Add(newUrlRecord);
            await _context.SaveChangesAsync();

            newUrlRecord.ShortenedUrl = $"{BASEURL}/{Base64.Encode(newUrlRecord.Id)}";
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id = newUrlRecord.Id });
        }
    }

    public class Base64
    {
        private readonly static string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private readonly static int Base = Alphabet.Length;

        public static string Encode(int n)
        {
            if (n == 0) return Alphabet[0].ToString();
            var s = new StringBuilder();
            while (n > 0)
            {
                s.Insert(0, Alphabet[n % Base]);
                n = n / Base;
            }
            return s.ToString();
        }

        public static int Decode(string s)
        {
            var n = 0;
            foreach (var c in s)
            {
                n = (n * Base) + Alphabet.IndexOf(c);
            }
            return n;
        }

    }
}
