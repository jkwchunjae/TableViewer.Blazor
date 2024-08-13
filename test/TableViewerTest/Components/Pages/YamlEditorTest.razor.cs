using BlazorMonaco.Editor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TableViewerBlazor.Options;

namespace TableViewerTest.Components.Pages;

[Route("/yaml-editor")]
public partial class YamlEditorTest : ComponentBase
{
    [Inject] IJSRuntime Js { get; set; } = default!;

    private string yaml = GetYamlString();

    IMonacoEditorOption option = new MonacoEditorOption
    {
        Dimension =
        {
            Height = 700,
            Width = 600,
        },
        MiniMap = new EditorMinimapOptions
        {
            Enabled = false,
        },
        LineNumbers = RenderLineNumbersType.Off.ToString(),
        GlyphMargin = false,
        Folding = false,
        //ReadOnly = true,
    };

    private async Task OnDataChanged(string value)
    {
        await Js.InvokeVoidAsync("console.log", value);
    }

    private static string GetYamlString()
    {
        return "serverRequirements:\n  - attributes:\n    - containerName: default-container\n      envVars:\n      - name: ENV_NAME\n        value: ENV_VALUE\n      - name: HOST_IP\n        valueFrom:\n          fieldRef:\n            fieldPath: status.hostIP\n      imageSource: us-docker.pkg.dev/agones-images/examples/simple-game-server\n      imageVersion: \"0.22\"\n      requestResource:\n        limit: {}\n        request:\n          cpu: \"1\"\n          memory: ${memory}\n      volumeMounts:\n      - mountPath: /data\n        name: volume1\n    fleetName: simple-game-server\n    health:\n      failureThreshold: 3\n      initialDelaySeconds: 30\n      periodSeconds: 10\n    labels:\n      category: ${category}\n      map: main\n      matchRegion: as\n      version: 1.0.0\n    ports:\n    - containerPort: 7654\n      name: default\n      protocol: UDP\n    volumes:\n    - name: volume1\nosType: \"linux\"\nautoscale:\n  bufferSize: \"1\"\n  maxReplicas: 10\n  minReplicas: 1 ";
    }
}
