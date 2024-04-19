using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.Models;
using Demp.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	[Authorize] // Athenticated with Cookies schema
	public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public EmployeeController( IUnitOfWork unitOfWork, 
                                    IWebHostEnvironment env,
                                    IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _env = env;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string searchInput)
        {
            var employees = Enumerable.Empty<Employee>();
            if (searchInput == null)
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            else
                employees = _unitOfWork.EmployeeRepository.SearchByName(searchInput.ToLower());
            var employeeViewModel = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(employeeViewModel);
        }
		public async Task<IActionResult> Search(string searchInput)
		{
			var employees = Enumerable.Empty<Employee>();
			if (searchInput == null)
				employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
			else
				employees = _unitOfWork.EmployeeRepository.SearchByName(searchInput.ToLower());
			var employeeViewModel = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
			return PartialView("PartialViews/EmployeesTablePartialView", employeeViewModel);
		}
		public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeViewModel)
        {
            // Upload file
            if (!ModelState.IsValid)
                return View(employeeViewModel);
            try
            {
                employeeViewModel.ImageName = DocumentSettings.UploadFile(employeeViewModel.Image, "images");
                // AutoMapper
                var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeViewModel);
                 await _unitOfWork.EmployeeRepository.AddAsync(employee);
                var rows = await _unitOfWork.CompleteAsync();
                if (rows > 0)
                    TempData["Message"] = "Employee Added Successfully";
                else
                    TempData["Message"] = "An Error Occured Employee Not Created !!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "Adding new Employee faild");
                return View(employeeViewModel);
            }

        }


        public async Task<IActionResult> Details(int? id , string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest();
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            var employeeViewModel = _mapper.Map<Employee , EmployeeViewModel>(employee);
            if (employee is null)
                return NotFound();
            return View(viewName , employeeViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id , "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id ,EmployeeViewModel employeeViewModel)
        {
            if(id != employeeViewModel.Id)
                return BadRequest();
            try
            {
                employeeViewModel.ImageName = DocumentSettings.UploadFile(employeeViewModel.Image , "images");
                var employee = _mapper.Map<EmployeeViewModel, Employee>(employeeViewModel);
                _unitOfWork.EmployeeRepository.Update(employee);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));

            }catch(Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "Updating employee is faild");
                return View(employeeViewModel);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id , "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeViewModel)
        {
            try
            {
                var employee = _mapper.Map<EmployeeViewModel , Employee>(employeeViewModel);    
                 _unitOfWork.EmployeeRepository.Delete(employee);
                var rows =  await _unitOfWork.CompleteAsync();
                if (rows > 0 && employeeViewModel.ImageName is not null)
                    DocumentSettings.DeleteFile(employeeViewModel.ImageName, "images");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "Deleting employee faild");

                return View(employeeViewModel);
            }
        }
    }
}
