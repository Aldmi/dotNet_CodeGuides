
//Встряхнуть массив- рандомизация массива.


 void foo(Span<int> array)
 {
     array[0] = 10;

 }



int[] baseArray = Enumerable.Range(1, 200).ToArray();

 foo(baseArray);

 //Console.Read();
 
void Shuffle(int[] numbers)
{
    for (int i = 0; i < numbers.Length; i++)
    {
        int randomIndex = Random.Shared.Next(0, numbers.Length);
        (numbers[randomIndex], numbers[i]) = (numbers[i], numbers[randomIndex]); //SWAP 2 items
    }
}

//Рандомизировать массив в C# с помощью GUID (создается новый массив)
static int[] RandomizeWithOrderByAndGuid(int[] array) =>
    array.OrderBy(x => Guid.NewGuid()).ToArray();

//Рандомизация массива в C# с использованием класса Random (создается новый массив)
static int[] RandomizeWithOrderByAndRandom(int[] array) =>
    array.OrderBy(x => Random.Shared.Next()).ToArray();

//Рандомизировать массив  алгоритм Фишера-етса (Работаем внутри текущего массива)
static int[] RandomizeWithFisherYates(int[] array)
{
    for (int i = 0; i < array.Length; i++)
    {
        int randomIndex = Random.Shared.Next(i, array.Length);//вычислим рандомный индекс в границах оставшейся части массива.
        (array[randomIndex], array[i]) = (array[i], array[randomIndex]); //SWAP 2 items
    }
    return array;
}


var shuffleArray = RandomizeWithFisherYates(baseArray);

Console.WriteLine(string.Join(',',shuffleArray));