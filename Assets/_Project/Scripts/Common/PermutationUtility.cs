using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// ChatGPT
/// </summary>
public static class PermutationUtility
{
    public static HashSet<string> GeneratePermutationsFromString(string input, bool keepLastChar, int numberNeedKeep)
    {
        HashSet<string> result = new HashSet<string>();
        string[] numbers = input.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string numStr in numbers)
        {
            if (numStr.Length == 1) // Nếu chỉ có một chữ số thì không cần hoán vị
            {
                result.Add(numStr);
                continue;
            }

            string fixedDigit = ""; // Số cố định ở cuối (nếu có)
            List<char> digits = new List<char>();

            // Kiểm tra xem số có chứa `numberNeedKeep` hay không
            bool containsNumberNeedKeep = numStr.Contains(numberNeedKeep.ToString());

            if (keepLastChar && containsNumberNeedKeep)
            {
                fixedDigit = numberNeedKeep.ToString();
            }

            // Tách các chữ số còn lại để hoán vị
            foreach (char c in numStr)
            {
                if (fixedDigit == "" || c.ToString() != fixedDigit)
                {
                    digits.Add(c);
                }
            }

            // Nếu không cần giữ số cuối cùng, hoán vị toàn bộ
            if (!keepLastChar || !containsNumberNeedKeep)
            {
                result.UnionWith(GeneratePermutations(numStr.ToCharArray()));
                continue;
            }

            // Nếu có số cần giữ cố định
            HashSet<string> permutations = GeneratePermutations(digits);
            foreach (string perm in permutations)
            {
                result.Add(perm + fixedDigit);
            }
        }

        return result;
    }

    private static HashSet<string> GeneratePermutations(IEnumerable<char> numbers)
    {
        HashSet<string> result = new HashSet<string>();
        Generate(new List<char>(numbers), new List<char>(), new bool[numbers.Count()], result);
        return result;
    }

    private static void Generate(List<char> nums, List<char> current, bool[] used, HashSet<string> result)
    {
        if (current.Count == nums.Count)
        {
            result.Add(new string(current.ToArray()));
            return;
        }

        for (int i = 0; i < nums.Count; i++)
        {
            if (!used[i])
            {
                used[i] = true;
                current.Add(nums[i]);

                Generate(nums, current, used, result);

                current.RemoveAt(current.Count - 1);
                used[i] = false;
            }
        }
    }

    public static string ConvertToString(HashSet<string> permutations)
    {
        return string.Join(", ", permutations);
    }
}
