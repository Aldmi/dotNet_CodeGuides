﻿using System.Collections.Generic;
using System.Linq;
using App.Infrastructure.DataAccess;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace App
{
    public sealed class StudentController
    {
        private readonly SchoolContext _context;
        private readonly StudentRepository _repository;

        public StudentController(SchoolContext context)
        {
            _context = context;
            _repository = new StudentRepository(context);
        }

        public List<Student> GetAll() => _repository.GetAll();
        
        public string CheckStudentFavoriteCourse(long studentId, long courseId)
        {
            Student student = _repository.GetById(studentId);
            if (student == null)
                return "Student not found";

            Course course = Course.FromId(courseId);
            if (course == null)
                return "Course not found";

            return student.FavoriteCourse == course ? "Yes" : "No";
        }

        public string EnrollStudent(long studentId, long courseId, Grade grade)
        {
            Student student = _repository.GetById(studentId);
            if (student == null)
                return "Student not found";

            Course course = Course.FromId(courseId);
            if (course == null)
                return "Course not found";

            string result = student.EnrollIn(course, grade);

            _context.SaveChanges();

            return result;
        }

        public string DisenrollStudent(long studentId, long courseId)
        {
            Student student = _repository.GetById(studentId);
            if (student == null)
                return "Student not found";
            
            Course course = _context.Courses.FirstOrDefault(c => c.Id == courseId);
            if (course == null)
                return "Course not found";

            student.Disenroll(course);

            _context.SaveChanges();

            return "OK";
        }

        public string RegisterStudent(
            string firstName, string lastName, long nameSuffixId, string email,
            long favoriteCourseId, Grade favoriteCourseGrade)
        {
            //Можно добавить студента с новым курсом.
            // Course favoriteCourse = Course.CreateNew("dsadfsada");
            // if (favoriteCourse == null)
            //     return "Course not found";
            
            Course favoriteCourse = _context.Courses.FirstOrDefault(course => course.Id == favoriteCourseId); //EntityState.Unchanged - т.к. извлекли из бд сущность и ее не меняли.
            if (favoriteCourse == null)
                return "Course not found";
            
            Suffix suffix = Suffix.FromId(nameSuffixId); //Detached - не трекается, т.к. новая сущность и если не поменять  State на Unchanged, то будет попытка добавить в БД. Но т.к. Suffix инициализируется при создании БД, добавлять новый не надо. 
            if (suffix == null)
                return "Suffix not found";
            
            _context.Entry(suffix).State = EntityState.Unchanged;
            
            Result<Email> emailResult = Email.Create(email);
            if (emailResult.IsFailure)
                return emailResult.Error;

            Result<Name> nameResult = Name.Create(firstName, lastName, suffix);
            if (nameResult.IsFailure)
                return nameResult.Error;

            var student = new Student(
                nameResult.Value,
                emailResult.Value,
                favoriteCourse,
                favoriteCourseGrade);
            
            _repository.Save(student);
            _context.SaveChanges();

            return "OK";
        }

        public string EditPersonalInfo(
            long studentId, string firstName, string lastName, long nameSuffixId,
            string email, long favoriteCourseId)
        {
            Student student = _repository.GetById(studentId);
            if (student == null)
                return "Student not found";

            Course favoriteCourse = Course.FromId(favoriteCourseId);
            if (favoriteCourse == null)
                return "Course not found";

            Suffix suffix = Suffix.FromId(nameSuffixId);
            if (suffix == null)
                return "Suffix not found";

            Result<Email> emailResult = Email.Create(email);
            if (emailResult.IsFailure)
                return emailResult.Error;

            Result<Name> nameResult = Name.Create(firstName, lastName, suffix);
            if (nameResult.IsFailure)
                return nameResult.Error;

            student.EditPersonalInfo(nameResult.Value, emailResult.Value, favoriteCourse);

            _context.SaveChanges();

            return "OK";
        }
    }
}
