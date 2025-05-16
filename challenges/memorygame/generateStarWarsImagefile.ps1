# PowerShell script to generate starwars-images.json with name and image fields for IDs 1-32
$results = @()

for ($id = 1; $id -le 32; $id++) {
    $url = "https://akabab.github.io/starwars-api/api/id/$id.json"
    try {
        $response = Invoke-RestMethod -Uri $url -Method Get -ErrorAction Stop
        $obj = [PSCustomObject]@{
            id    = $response.id
            name  = $response.name
            image = $response.image
        }
        $results += $obj
    } catch {
        Write-Warning ("Failed to fetch")
    }
}

$results | ConvertTo-Json -Depth 3 | Set-Content -Encoding UTF8 starwars-images.json
Write-Host "starwars-images.json generated with $($results.Count) entries."
