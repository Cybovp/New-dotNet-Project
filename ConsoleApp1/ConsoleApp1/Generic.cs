using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Person(string firstname, string lastname)
        {
            FirstName = firstname;
            LastName = lastname;
        }
    }
    class Salary
    {
        public string Name { get; set; }
        public string Salaries { get; set; }
        public Salary(string name, string salaries) {
            Name = name;
            Salaries = salaries;
        }
    }
    static class EnumrableExtensions
    {
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
                if (predicate(item)) yield return item;
        }
        public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> projection)
        {
            foreach (var item in source)
                yield return projection(item);
        }
        public static TSource MyFirst<TSource> (this IEnumerable<TSource> source)
        {
            foreach (var item in source)
                    return item;
            throw new InvalidOperationException();
        }
        public static TSource MyFirst<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (var item in source)
                if (predicate(item))
                    return item;
            throw new InvalidOperationException();
        }
        public static bool MyAny<TSource>(this IEnumerable<TSource> source)
        {
            foreach (var item in source)
                return true;
            return false;
        }
        public static bool MyAny<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item)) return true;
            }
            return false;
        }
        public static bool MyAll<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach(var item in source)
                if (!predicate(item)) return false;
            return true;
        }
        public static int MyCount<T>(this IEnumerable<T> source)
        {
            var collection = source as ICollection<T>;
            if (collection != null)
                return collection.Count;

            var count = 0;
            foreach (var item in source)
                count++;
            return count;

        }
        public static int MyCount<T>(this IEnumerable<T> source, Func<T,bool> predicate)
        {
            var count = 0;
            foreach (var item in source)
                if(predicate(item))
                    count++;
            return count;
        }
        public static TAcc MyAggregate<TAcc, TSource>(this IEnumerable<TSource> source, TAcc seed, Func<TAcc, TSource, TAcc> fold)
        {
            var value = seed;
            foreach(var item in source)
                value = fold(value, item);

            return value;
        }
        public static IEnumerable<T> MyConcat<T>(this IEnumerable<T> firstSource, IEnumerable<T> secondSource)
        {
            foreach(var i in firstSource)
                yield return i;
            foreach(var i in secondSource)
                yield return i;
        }
        public static IEnumerable<T> MyUnion<T>(this IEnumerable<T> firstSource, IEnumerable<T> secondSource, IEqualityComparer<T> comparer)
        {
            var hashset = new HashSet<T>(comparer);

            foreach(var item in firstSource)
                if(hashset.Add(item))
                    yield return item;

            foreach (var item in secondSource)
                if (hashset.Add(item))
                    yield return item;
        }
        //overload union method: the same but not required comparor
        public static IEnumerable<T> MyUnion<T>(this IEnumerable<T> firstSource, IEnumerable<T> secondSource)
        {
            return MyUnion(firstSource, secondSource, EqualityComparer<T>.Default);
        }
        public static IEnumerable<T> MyExcept<T>(this IEnumerable<T> firstSource, IEnumerable<T> secondSource)
        {
            var blacklist = new HashSet<T>(secondSource);

            foreach (var item in firstSource)
                if (blacklist.Add(item))
                    yield return item;
        }
        public static IEnumerable<T> MyIntercect<T>(this IEnumerable<T> firstSource, IEnumerable<T> secondSource)
        {
            var hashset = new HashSet<T>(secondSource);

            foreach (var item in firstSource)
                //if (!hashset.Add(item))
                if (hashset.Remove(item))
                    yield return item;
        }  
        public static Dictionary<TKey, TValue> MyToDictionary<TSource, TKey, TValue>(
            this IEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector,
            Func<TSource, TValue> valueSelector)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var item in source)
                dictionary.Add(keySelector(item), valueSelector(item));
            return dictionary;
        }
        public static IEnumerable<TResult> MyCast<TResult>(this IEnumerable source)
        {
           foreach(var item in source)
                yield return (TResult)item;
        }
        public static IEnumerable<TResult> MyOfType<TResult>(this IEnumerable source)
        {
            foreach (var item in source)
                if(item is TResult) 
                    yield return (TResult)item;
        }
        public static IEnumerable<TResult> MyJoin<TResult, TOuter, TInner, TKey>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> projection,
            IEqualityComparer<TKey> comparer)
        {
            foreach( var outerItem in outer)
            {
                var outerkey = outerKeySelector(outerItem);
                foreach( var innerItem in inner)
                {
                    var innerkey = innerKeySelector(innerItem);
                    if (comparer.Equals(outerkey, innerkey))
                        yield return projection(outerItem, innerItem);
                }
            }
        }
        //overload join method: the same but not required comparor
        public static IEnumerable<TResult> MyJoin<TResult, TOuter, TInner, TKey>(
            this IEnumerable<TOuter> outer,
            IEnumerable<TInner> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> projection)
        {
            return MyJoin(outer, inner, outerKeySelector, innerKeySelector, projection, EqualityComparer<TKey>.Default);
        }
        public static bool MySequenceEqual<TSource>(this IEnumerable<TSource> source1, IEnumerable<TSource> source2)
        {
            return MySequenceEqual(source1, source2, EqualityComparer<TSource>.Default);

        }
        public static bool MySequenceEqual<TSource>(this IEnumerable<TSource> source1, IEnumerable<TSource> source2, IEqualityComparer<TSource> comparer)
        {
            using var source1Enum = source1.GetEnumerator();
            using var source2Enum = source2.GetEnumerator();
            while (true)
            {
                var source1HasMore = source1Enum.MoveNext();
                var source2HasMore = source2Enum.MoveNext();

                if (!source1HasMore && !source2HasMore)
                    return true;

                if (source1HasMore != source2HasMore)
                    return false;

                if (!comparer.Equals(source1Enum.Current, source2Enum.Current))
                    return false;
            }
        }
        public static IEnumerable<TResult> MyZip<TSource, TSecond,  TResult>(
            this IEnumerable<TSource> source1, 
            IEnumerable<TSecond> source2,
            Func<TSource, TSecond, TResult> projection)
        {
            using var source1Enum = source1.GetEnumerator();
            using var source2Enum = source2.GetEnumerator();
            if (source1Enum.MoveNext() && source2Enum.MoveNext())
                yield return projection(source1Enum.Current, source2Enum.Current);
        }
    }
    class Generic
    {
        public static void Main(string[] args)
        {
            var listPerson = new List<Person>
            { 
                new Person("Binh","Vu"), 
                new Person("binh", "vu"), 
                new Person("Vinh", "Vu"), 
                new Person("BinH", "Bu") 
            };
            var listSalary = new List<Salary>
            {
                new Salary("Binh","2000$"),
                new Salary("Vinh","2100$"),
                new Salary("binh","2200$"),
                new Salary("BinH","2300$"),
            };
            //var personStartWithB = Filter(listPerson, x => x.FirstName.StartsWith("b", StringComparison.CurrentCultureIgnoreCase ));

            // In LINQ Filter = Where and Map = Select

            // use select and where
            var firstnameStartWithB = listPerson
                .Filter(x => x.FirstName.StartsWith("b", StringComparison.CurrentCultureIgnoreCase))
                .Map(x => x.FirstName);
            foreach ( var person in firstnameStartWithB)
            {
                Console.WriteLine(person);
            }

            Console.WriteLine("--------------");

            //use dictionary
            var firstnameStartWithB2 = listPerson.MyToDictionary(x => x.FirstName, v => v.FirstName.StartsWith("b", StringComparison.CurrentCultureIgnoreCase));
            foreach (var person in firstnameStartWithB2)
            {
                Console.WriteLine(person);
            }

            // In LINQ Filter = Where and Map = Select

            //var list = new List<int>();
            //for ( var i =1; i <= 10; i++)
            //{
            //    list.Add(i);
            //}
            // Here is how to use static method in LINQ to replace above lines of code
            var list = Enumerable.Range(0, 10);

            var mod2 =
                //.Where(i => i % 2 == 0)
                //.Select(i => i*i);
                // Replace above code to SQL syntax
                from p in list
                where p % 2 == 0
                select p * p;
            foreach ( var i in mod2)
            {
                Console.WriteLine(i);
            }

            //Test myFirst: return the first element(with or without condition)
            var list2 = new List<int>{22};
            Console.WriteLine(list.MyFirst(x => x%2 == 0));
            
            //Test MyAny: return any element(with or without condition)
            if(list2.MyAny(x => x%11 == 0)) { Console.WriteLine("yes"); }

            //Test MyAggregate
            var list3 = new List<int>() {6,7,8,9,10,11,12,13};
            var max = list3.MyAggregate(1, (x, y) => x > y ? x : y);
            Console.WriteLine(max);

            //Test MyConcat: return the combination of two IEnumerable
            foreach (var item in list.MyConcat(list3)) { Console.WriteLine(item); }

            //Test MyUnion: return the unique elements of two IEnumerable
            foreach (var item in list.MyUnion(list3)) { Console.WriteLine(item); }

            var StringList = new List<string>() { "Hello", "World","Damn","You"};
            var StringList2 = new List<string>() { "HELLO", "WORLD", "YOU", "yoU" };

            foreach (var item in StringList.MyUnion(StringList2)) { Console.WriteLine("{0}, ",item); };

            Console.WriteLine("--------------");

            foreach (var item in StringList.MyUnion(StringList2, StringComparer.CurrentCultureIgnoreCase)) { Console.WriteLine("{0}, ", item); };

            Console.WriteLine("--------------");

            //Test MyExcept: return the elements in the first IEnumerable that doesn't exist in the second IEnumerable
            foreach (var item in list.MyExcept(list3)) { Console.WriteLine(item); }

            Console.WriteLine("--------------");

            //Test MyIntercect: return the elements exists in both IEnumerable
            foreach (var item in list.MyIntercect(list3)) { Console.WriteLine(item); }

            Console.WriteLine("--------------");

            //Test MyJoin: Join 2 IEnumerable by the key, same as JOIN in SQL
            var salaryy = listPerson.MyJoin(
                listSalary,
                x => x.FirstName,
                y => y.Name,
                (p,s) => p.FirstName + " have salary: " + s.Salaries);
            foreach(var item in salaryy) {  Console.WriteLine(item); }

            Console.WriteLine("--------------");

            //groupby is deferred: means it will wait till the data is used to actuallly executed: as bellow
            //although we add item after we create a groupby, the added item still exists in groupby
            //as groupby is executed when the foreach loop starts
            var groupby = listPerson.GroupBy(x => x.FirstName.Length > 0 ? x.FirstName[0] : ' ');
            listPerson.Add(new Person("Hieu", "Tran"));
            foreach (var item in groupby)
            {
                Console.WriteLine("Name start with: " + item.Key);
                foreach (var item2 in item)
                {
                    Console.WriteLine(item2.FirstName + item2.LastName);
                }
            }
            Console.WriteLine("--------------");

            //look up is not deferred: means once you create a lookup, it will remain the same
            var lookup = listPerson.ToLookup(x => x.FirstName.Length > 0 ? x.FirstName[0] : ' ');
            listPerson.Add(new Person("hieu", "tran"));
            foreach(var item in lookup) 
            {
                Console.WriteLine("Name start with: "+item.Key);
                foreach (var item2 in item)
                { 
                    Console.WriteLine(item2.FirstName + item2.LastName); 
                }
            }

            //Test MySequenceEqual
            var newlist = new List<string> { "binh", "vu", "dep", "trai" };
            var newlist2 = new List<string> { "binh", "vu", "dep", "trai" };
            var newlist3 = new List<string> { "binh", "vu", "trai", "dep" };
            var newlist4 = new List<string> { "binh", "vu", "dep", "trai","vai"};

            if (newlist.MySequenceEqual(newlist2))
                Console.WriteLine("They are equal");
            else Console.WriteLine("They are not equal");

            if (newlist.MySequenceEqual(newlist3))
                Console.WriteLine("They are equal");
            else Console.WriteLine("They are not equal");

            if (newlist.MySequenceEqual(newlist4))
                Console.WriteLine("They are equal");
            else Console.WriteLine("They are not equal");

            //Test anonymous type with LINQ through lambda
            //The code below actually create a class name "group" with structure having: firstLetter, count and AverageNameLength
            var group = listPerson.GroupBy(x => x.FirstName[0], (key, person) => new
            {
                firstLetter = key,
                count = person.Count(),
                AverageNameLength = person.Select(p => p.FirstName.Length).Average()
            });
            foreach (var item in group)
                Console.WriteLine("For people with first name start with {0}, there are {1} of them, with the avarage first name length of {2}",
                    item.firstLetter,
                    item.count,
                    item.AverageNameLength);
        }
    }
}
