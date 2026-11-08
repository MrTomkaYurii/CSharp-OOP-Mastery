import os

ps_script = r"""
Add-Type -AssemblyName System.Runtime.WindowsRuntime
$null = [Windows.Storage.StorageFile, Windows.Storage, ContentType = WindowsRuntime]
$null = [Windows.Graphics.Imaging.BitmapDecoder, Windows.Graphics.Imaging, ContentType = WindowsRuntime]
$null = [Windows.Media.Ocr.OcrEngine, Windows.Foundation, ContentType = WindowsRuntime]

$asTaskGeneric = ([System.WindowsRuntimeSystemExtensions].GetMethods() | Where-Object { 
    $_.Name -eq 'AsTask' -and $_.GetParameters().Count -eq 1 -and $_.GetParameters()[0].ParameterType.Name -like 'IAsyncOperation*' 
})[0]

function Await($asyncOp, $type) {
    $m = $asTaskGeneric.MakeGenericMethod($type)
    $t = $m.Invoke($null, @($asyncOp))
    $t.Wait()
    return $t.Result
}

$engine = [Windows.Media.Ocr.OcrEngine]::TryCreateFromUserProfileLanguages()
if (-not $engine) {
    Write-Error "OCR Engine not available"
    exit 1
}

$files = Get-ChildItem -Path "temp_slides_png" -Filter "slide_*.png" | Sort-Object Name
foreach ($f in $files) {
    $num = $f.BaseName
    $outFile = "temp_slides_png/$num.txt"
    if (Test-Path $outFile) { continue }
    
    try {
        $file = Await ([Windows.Storage.StorageFile]::GetFileFromPathAsync($f.FullName)) ([Windows.Storage.StorageFile])
        $stream = Await ($file.OpenAsync([Windows.Storage.FileAccessMode]::Read)) ([Windows.Storage.Streams.IRandomAccessStream])
        $decoder = Await ([Windows.Graphics.Imaging.BitmapDecoder]::CreateAsync($stream)) ([Windows.Graphics.Imaging.BitmapDecoder])
        $bitmap = Await ($decoder.GetSoftwareBitmapAsync()) ([Windows.Graphics.Imaging.SoftwareBitmap])
        $ocrResult = Await ($engine.RecognizeAsync($bitmap)) ([Windows.Media.Ocr.OcrResult])
        
        $lines = $ocrResult.Lines | ForEach-Object { $_.Text }
        $lines -join "`n" | Out-File -FilePath $outFile -Encoding utf8
        Write-Host "Processed $($num)"
    } catch {
        Write-Host "Error on $($num)"
    }
}
Write-Host "OCR extraction complete!"
"""

with open("run_ocr_all.ps1", "w", encoding="utf-8") as f:
    f.write(ps_script)

print("Saved run_ocr_all.ps1 successfully")
