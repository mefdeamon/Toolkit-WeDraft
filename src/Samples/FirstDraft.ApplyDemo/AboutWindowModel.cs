using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows;

namespace FirstDraft.ApplyDemo
{

    public class AboutWindowModel : ObservableObject
    {
        public RelayCommand OpenAboutCommand => new RelayCommand(() =>
        {
            AboutWindow.ShowInstance();
            Search();
        });



        public ObservableCollection<GitHubRelease> Releases { get; set; } = new ObservableCollection<GitHubRelease>();

        public async void Search()
        {
            HasNewVersion = false;
            FileInfo file = new FileInfo("github.current.version.fd");
            if (file.Exists)
            {
                var json = File.ReadAllText(file.FullName);
                try
                {
                    CurrentVersion = JsonSerializer.Deserialize<GitHubRelease>(json);
                }
                catch (Exception)
                {
                }
            }

            string owner = "mefdeamon"; // GitHub 仓库的所有者
            string repo = "Toolkit-WeDraft"; // GitHub 仓库的名称

            // GitHub API 的 URL
            string url = $"https://api.github.com/repos/{owner}/{repo}/releases";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0")); // 添加用户代理，GitHub API 需要

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    // 使用 JSON 反序列化将响应内容转换为 Release 对象列表
                    var releases = JsonSerializer.Deserialize<List<GitHubRelease>>(responseBody);

                    // 输出每个发布的信息
                    if (releases != null && releases.Count > 0)
                    {
                        Releases.Clear();

                        foreach (var release in releases)
                        {
                            Releases.Add(release);
                        }

                        // 检查是否存在更新的
                        if (CurrentVersion != null)
                        {
                            HasNewVersion = Releases.Where(t => t.created_at > CurrentVersion.created_at).Any();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"获取发布信息时发生错误: {ex.Message}");
                }
            }
        }


        public async void Download(GitHubRelease release, Action<bool, string> callback)
        {
            if (release != null && release.assets.Count > 0)
            {
                string url = release.assets[0].browser_download_url;
                string filePath =System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"release_pkg", release.assets[0].name);

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCode();
                            
                            using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                                          fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                            {
                                await contentStream.CopyToAsync(fileStream);
                            }
                        }

                        callback?.Invoke(true, "文件下载完成");
                    }
                    catch (Exception ex)
                    {
                        callback?.Invoke(false, "遇到错误，消息为：" + ex.Message);
                    }
                }
            }
            else
            {
                callback?.Invoke(false, "下载信息不存在");
            }
        }



        private GitHubRelease lastRelease;

        public GitHubRelease LastRelease
        {
            get { return lastRelease; }
            set { SetProperty(ref lastRelease, value); }
        }

        private GitHubRelease currentVersion;

        public GitHubRelease CurrentVersion
        {
            get { return currentVersion; }
            set { SetProperty(ref currentVersion, value); }
        }



        private Boolean hasNewVersion;

        public Boolean HasNewVersion
        {
            get { return hasNewVersion; }
            set { SetProperty(ref hasNewVersion, value); }
        }


        public RelayCommand UpdateCommand => new RelayCommand(() =>
        {
            if (Releases.Any())
            {
                var release = Releases.OrderBy(t => t.created_at).LastOrDefault();
                if (release != null)
                {
                    Download(release, (status, msg) =>
                    {
                        MessageBox.Show($"{status}  +  {msg}");
                    });
                }
            }
        });

    }
}
