using System.Diagnostics;

namespace WhoKnowsBackend.Scripts  // Ensure this matches the namespace in your controller
{
    public static class PythonScriptExecutor  // Changed to a static class for better organization
    {
        public static string ExecutePythonScript(string arguments)
        {



            string pythonPath = "python3"; // You can replace this with the full path if Python is not in PATH.

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = $"/home/azureuser/Backend/publish/Scripts/fetchData.py, \"{arguments}\"",

                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using (Process process = Process.Start(startInfo))
                {
                    if (process == null)
                    {
                        throw new InvalidOperationException("Failed to start the Python process.");
                    }

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new InvalidOperationException($"Error executing script: {error}");
                    }

                    return output;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while executing the Python script: {ex.Message}", ex);
            }
        }
    }
}