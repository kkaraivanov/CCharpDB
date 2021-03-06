﻿namespace PetStore.Data.Model.PetModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using PetSore.Data.Common.Enumerators;
    using StoreModel;
    using static DataValidationAttribute;

    public class Pet
    {
        [Key]
        public int Id { get; set; }

        public Gender Gender { get; set; }

        public int BreadId { get; set; }
        public Bread Bread { get; set; }


        public DateTime Birth { get; set; }

        public decimal Price { get; set; }

        public int ImageId { get; set; }
        public Image Image { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int? OrderId { get; set; }
        public Order Order { get; set; }

        [MaxLength(DescriptionMaxLenght)]
        public string Description { get; set; }
    }
}