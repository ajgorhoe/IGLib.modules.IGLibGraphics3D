using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace IGLib.Tests.Base.SampleClasses
{


    #region ClassesForTests

    public class BaseClass
    {
        public BaseClass()
        {
            ID = 11;
            Name = $"{GetType().Name} object";
        }
        public override string ToString()
        {
            return $"{GetType().Name}: ID = {ID}, Name = \"{Name}\".";
        }
        public int ID { get; set; }
        public string Name { get; set; }

        // Implementations for equality comparisson:
        public override bool Equals(object obj) => this.Equals(obj as BaseClass);
        public bool Equals(BaseClass compared)
        {
            if (compared is null)
            {
                return false;
            }
            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, compared))
            {
                return true;
            }
            // If run-time types are not exactly the same, return false.
            if (this.GetType() != compared.GetType())
            {
                return false;
            }
            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return (ID == compared.ID) && (Name == compared.Name);
        }
        public override int GetHashCode() => (ID, Name).GetHashCode();
        public static bool operator ==(BaseClass lhs, BaseClass rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }
                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }
        public static bool operator !=(BaseClass lhs, BaseClass rhs) => !(lhs == rhs);
    }


    public class DerivedClass : BaseClass
    {
        public DerivedClass()
        {
            ID = 12;
            Name = $"{GetType().Name} object";
            Description = $"This is an instance of {GetType().Name}.";
        }
        public override string ToString()
        {
            return $"{GetType().Name}: ID = {ID}, Name = \"{Name}\", Description = \"{Description}\".";
        }
        public string Description { get; set; }
        public static implicit operator DerivedClass(ImplicitlyConvertibleToDerived source)
        {
            if (source == null) return null;
            return new DerivedClass
            {
                ID = source.MyId2,
                Name = source.MyName2,
                Description = source.MyDescription2,
            };
        }
        public const string DescriptionWhenConvertedFromIncompatibleClass =
            "Converted from a class that do not have Description equivalent.";
        public static explicit operator DerivedClass(ExplicitlyConvertibleToDerived source)
        {
            if (source == null) return null;
            return new DerivedClass
            {
                ID = source.MyId1,
                Name = source.MyName1,
                Description = DescriptionWhenConvertedFromIncompatibleClass
            };
        }

        // Implementations for equality comparisson:
        public override bool Equals(object obj) => this.Equals(obj as DerivedClass);
        public bool Equals(DerivedClass compared)
        {
            if (compared is null)
            {
                return false;
            }
            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, compared))
            {
                return true;
            }
            // If run-time types are not exactly the same, return false.
            if (this.GetType() != compared.GetType())
            {
                return false;
            }
            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return (ID == compared.ID) && (Name == compared.Name && Description == compared.Description);
        }
        public override int GetHashCode() => (ID, Name, Description).GetHashCode();
        public static bool operator ==(DerivedClass lhs, DerivedClass rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }
                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }
        public static bool operator !=(DerivedClass lhs, DerivedClass rhs) => !(lhs == rhs);
    }

    public class ImplicitlyConvertibleToDerived
    {
        public ImplicitlyConvertibleToDerived()
        {
            MyId2 = 111;
            MyName2 = $"{GetType().Name} object";
            MyDescription2 = $"This is an instance of {GetType().Name}.";
        }
        public override string ToString()
        {
            return $"{GetType().Name}: MyId2 = {MyId2}, MyName2 = \"{MyName2}\", MyDescription2 \"{MyDescription2}\".";
        }
        public int MyId2 { get; set; }
        public string MyName2 { get; set; }
        public string MyDescription2 { get; set; }

        // Implementations for equality comparisson:
        public override bool Equals(object obj) => this.Equals(obj as ImplicitlyConvertibleToDerived);
        public bool Equals(ImplicitlyConvertibleToDerived compared)
        {
            if (compared is null)
            {
                return false;
            }
            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, compared))
            {
                return true;
            }
            // If run-time types are not exactly the same, return false.
            if (this.GetType() != compared.GetType())
            {
                return false;
            }
            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return (MyId2 == compared.MyId2) && (MyName2 == compared.MyName2) && (MyDescription2 == compared.MyDescription2);
        }
        public override int GetHashCode() => (MyId2, MyName2, MyDescription2).GetHashCode();
        public static bool operator ==(ImplicitlyConvertibleToDerived lhs, ImplicitlyConvertibleToDerived rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }
                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }
        public static bool operator !=(ImplicitlyConvertibleToDerived lhs, ImplicitlyConvertibleToDerived rhs) => !(lhs == rhs);
    }


    public class ExplicitlyConvertibleToDerived
    {
        public ExplicitlyConvertibleToDerived()
        {
            MyId1 = 112;
            MyName1 = $"{GetType().Name} object";
        }
        public override string ToString()
        {
            return $"{GetType().Name}: ID = {MyId1}, Name = \"{MyName1}\".";
        }
        public int MyId1 { get; set; }
        public string MyName1 { get; set; }

        // Implementations for equality comparisson:
        public override bool Equals(object obj) => this.Equals(obj as ExplicitlyConvertibleToDerived);
        public bool Equals(ExplicitlyConvertibleToDerived compared)
        {
            if (compared is null)
            {
                return false;
            }
            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, compared))
            {
                return true;
            }
            // If run-time types are not exactly the same, return false.
            if (this.GetType() != compared.GetType())
            {
                return false;
            }
            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return (MyId1 == compared.MyId1) && (MyName1 == compared.MyName1);
        }
        public override int GetHashCode() => (MyId1, MyName1).GetHashCode();
        public static bool operator ==(ExplicitlyConvertibleToDerived lhs, ExplicitlyConvertibleToDerived rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }
                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }
        public static bool operator !=(ExplicitlyConvertibleToDerived lhs, ExplicitlyConvertibleToDerived rhs) => !(lhs == rhs);
    }


    public class ImplicitlyConvertibleFromDerived
    {
        public ImplicitlyConvertibleFromDerived()
        {
            MyId4 = 211;
            MyName4 = $"{GetType().Name} object";
            MyDescription4 = $"This is an instance of {GetType().Name}.";
        }
        public override string ToString()
        {
            return $"{GetType().Name}: MyId4 = {MyId4}, MyName4 = \"{MyName4}\", MyDescription4 \"{MyDescription4}\".";
        }
        public int MyId4 { get; set; }
        public string MyName4 { get; set; }
        public string MyDescription4 { get; set; }
        public static implicit operator ImplicitlyConvertibleFromDerived(DerivedClass source)
        {
            return new ImplicitlyConvertibleFromDerived
            {
                MyId4 = source.ID,
                MyName4 = source.Name,
                MyDescription4 = source.Description,
            };
        }

        // Implementations for equality comparisson:
        public override bool Equals(object obj) => this.Equals(obj as ImplicitlyConvertibleFromDerived);
        public bool Equals(ImplicitlyConvertibleFromDerived compared)
        {
            if (compared is null)
            {
                return false;
            }
            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, compared))
            {
                return true;
            }
            // If run-time types are not exactly the same, return false.
            if (this.GetType() != compared.GetType())
            {
                return false;
            }
            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return (MyId4 == compared.MyId4) && (MyName4 == compared.MyName4) && (MyDescription4 == compared.MyDescription4);
        }
        public override int GetHashCode() => (MyId4, MyName4, MyDescription4).GetHashCode();
        public static bool operator ==(ImplicitlyConvertibleFromDerived lhs, ImplicitlyConvertibleFromDerived rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }
                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }
        public static bool operator !=(ImplicitlyConvertibleFromDerived lhs, ImplicitlyConvertibleFromDerived rhs) => !(lhs == rhs);
    }



    public class ExplicitlyConvertibleFromDerived
    {
        public ExplicitlyConvertibleFromDerived()
        {
            MyId3 = 212;
            MyName3 = $"{GetType().Name} object";
        }
        public override string ToString()
        {
            return $"{GetType().Name}: ID = {MyId3}, Name = \"{MyName3}\".";
        }
        public int MyId3 { get; set; }
        public string MyName3 { get; set; }
        public static explicit operator ExplicitlyConvertibleFromDerived(DerivedClass source)
        {
            return new ExplicitlyConvertibleFromDerived
            {
                MyId3 = source.ID,
                MyName3 = source.Name,
            };
        }

        // Implementations for equality comparisson:
        public override bool Equals(object obj) => this.Equals(obj as ExplicitlyConvertibleFromDerived);
        public bool Equals(ExplicitlyConvertibleFromDerived compared)
        {
            if (compared is null)
            {
                return false;
            }
            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, compared))
            {
                return true;
            }
            // If run-time types are not exactly the same, return false.
            if (this.GetType() != compared.GetType())
            {
                return false;
            }
            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return (MyId3 == compared.MyId3) && (MyName3 == compared.MyName3);
        }
        public override int GetHashCode() => (MyId3, MyName3).GetHashCode();
        public static bool operator ==(ExplicitlyConvertibleFromDerived lhs, ExplicitlyConvertibleFromDerived rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }
                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }
        public static bool operator !=(ExplicitlyConvertibleFromDerived lhs, ExplicitlyConvertibleFromDerived rhs) => !(lhs == rhs);
    }


    // Collection classes:



    public class CustomEnumerable<T> : IEnumerable<T>
    {
        // Constructor that initializes the collection with multiple items via params
        public CustomEnumerable(params T[] items)
        {
            _items = new List<T>(items);
        }
        // Constructor that initializes the collection from an IEnumerable<T>
        public CustomEnumerable(IEnumerable<T> items)
        {
            _items = new List<T>(items);
        }
        private readonly List<T> _items;
        // Add method to add multiple items via params
        public void Add(params T[] items)
        {
            _items.AddRange(items);
        }
        // Add method to add items from an IEnumerable<T>
        public void Add(IEnumerable<T> items)
        {
            _items.AddRange(items);
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        // Explicit implementation of IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    public class CustomList<T> : IList<T>
    {
        // Constructor that initializes the collection with multiple items via params
        public CustomList(params T[] items)
        {
            _items = new List<T>(items);
        }
        // Constructor that initializes the collection from an IEnumerable<T>
        public CustomList(IEnumerable<T> items)
        {
            _items = new List<T>(items);
        }
        private readonly List<T> _items;
        // IList<T> Implementation
        public T this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }
        public int Count => _items.Count;
        public bool IsReadOnly => false;
        public void Add(T item)
        {
            _items.Add(item);
        }
        // Add method to add multiple items via params
        public void Add(params T[] items)
        {
            _items.AddRange(items);
        }
        // Add method to add items from an IEnumerable<T>
        public void Add(IEnumerable<T> items)
        {
            _items.AddRange(items);
        }
        public void Clear()
        {
            _items.Clear();
        }
        public bool Contains(T item)
        {
            return _items.Contains(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        public int IndexOf(T item)
        {
            return _items.IndexOf(item);
        }
        public void Insert(int index, T item)
        {
            _items.Insert(index, item);
        }
        public bool Remove(T item)
        {
            return _items.Remove(item);
        }
        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    #endregion ClassesForTests




}
