using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Models;
using Demp.BLL.Interfaces;
using Demp.BLL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{

    [Authorize] // Athenticated with Cookies schema
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public DepartmentController( IUnitOfWork unitOfWork, IWebHostEnvironment env , IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _env = env;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            var departmentsVM = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(departmentsVM);
        }
        public IActionResult Create() { 
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        { 
            if(!ModelState.IsValid) // Server side validation => Validate after request creation
                return View(departmentVM);
            try 
            {
                var department = _mapper.Map<DepartmentViewModel , Department>(departmentVM);
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                var rows = await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }catch(Exception ex)
            {
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "Adding new Department faild");
                return View(departmentVM);
            }
            
        }
        // /Department/Details/10
        // /Department/Details

        public async Task<IActionResult> Details(int? id , string ViewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest(); //400
            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if(department == null)
                return NotFound(); //404
            var DepartmentVM = _mapper.Map<Department , DepartmentViewModel>(department);
            return View(ViewName, DepartmentVM);
        }
        // /Department/Edit/10
        // /Department/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id , "Edit");
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Action filter to validat request is from my website not another tool like [Postmane]
        public async Task<IActionResult> Edit([FromRoute]int id ,DepartmentViewModel departmentVM)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var department = _mapper.Map<DepartmentViewModel , Department>(departmentVM);
                     _unitOfWork.DepartmentRepository.Update(department);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    if(_env.IsDevelopment())
                        ModelState.AddModelError(string.Empty , ex.Message);
                    else
                        ModelState.AddModelError(string.Empty, "Error Has Occured during updating the department");

                    return View(departmentVM);
                }
            }
            return View(departmentVM);

        }

        public async Task<IActionResult> Delete(int? id) {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DepartmentViewModel departmentVM)
        {
            try
            {
                var department = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Delete(department);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Approach 1 => Log Exception to developer Then, Display friendly error msg to user
                // Approach 2 => Display Error msg in form [asp-validation-summary]
                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "Error Has Occured during updating the department");

                return View(departmentVM);
            }
            
        }
    }
}
