import requests
from bs4 import BeautifulSoup
import argparse

def fetch_combined_paragraphs(keyword):
    url = f"https://en.wikipedia.org/wiki/{keyword}"
    try:
        response = requests.get(url)
        response.raise_for_status() 
    except requests.exceptions.RequestException as e:
        return f"Error fetching the page: {e}"
    
    soup = BeautifulSoup(response.text, "html.parser")
    title = soup.find('h1').get_text()
    paragraphs = soup.find_all('p')
    combined_text = title + " " + " ".join(p.get_text() for p in paragraphs[:10])
    
    return combined_text

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Fetch data")
    parser.add_argument("keyword", help="The keyword to search for.")
    args = parser.parse_args()
    result = fetch_combined_paragraphs(args.keyword)
    print(result)