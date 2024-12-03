import requests
from bs4 import BeautifulSoup
import argparse
import json
from datetime import datetime

def fetch_combined_paragraphs(keyword):
    url = f"https://en.wikipedia.org/wiki/{keyword}"
    try:
        response = requests.get(url)
        response.raise_for_status()  # Raise an HTTPError for bad responses (4xx and 5xx)
    except requests.exceptions.RequestException as e:
        return json.dumps([{
            "Title": "",
            "Url": url,
            "CreatedBy": "",
            "Language": "en",
            "LastUpdated": datetime.utcnow().isoformat(),
            "Content": f"Error fetching the page: {e}",
            "Id": -1
        }])
    
    soup = BeautifulSoup(response.text, "html.parser")
    title = soup.find('h1').get_text() if soup.find('h1') else "Unknown Title"
    paragraphs = soup.find_all('p')
    combined_text = " ".join(p.get_text() for p in paragraphs[:10])
    
    result = [{
        "Title": title,
        "Url": url,
        "CreatedBy": "",
        "Language": "en",
        "LastUpdated": datetime.utcnow().isoformat(),
        "Content": combined_text,
        "Id": -1
    }]
    
    return json.dumps(result)

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Fetch data")
    parser.add_argument("keyword", help="The keyword to search for.")
    args = parser.parse_args()
    result = fetch_combined_paragraphs(args.keyword)
    print(result)
