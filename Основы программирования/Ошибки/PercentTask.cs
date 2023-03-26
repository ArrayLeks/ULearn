public static double Calculate(string userInput)
{
    string[] array = userInput.Split(' ');
	double sum = Convert.ToDouble(array[0]);
	double percent = Convert.ToDouble(array[1]);
	double month = Convert.ToDouble(array[2]);
	if (month == 0) return sum;

	sum += sum * (percent / 12 * 0.01);

	return Calculate($"{sum} {percent} {month - 1}");
}