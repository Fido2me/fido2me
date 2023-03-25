namespace Fido2me
{
    public static class DotEnv
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(new[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);      

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}
