# Unity Material Usage Analyzer

Unity Material Usage Analyzer is a simple and efficient Unity Editor extension that helps optimize scenes that have become cluttered with a high number of materials, some of which may be underutilised.

This extension can identify these underutilised materials, and provide an overview of their usage, to help reduce the overall number of materials and improve scene performance.

## Features

- Provides a list of all materials being used in the scene, ordered by the number of times they are used, from least to most.
- Displays the total unique materials used count.
- Shows the total number of MeshRenderers in the scene.
- Allows setting a "usage threshold", so that only materials used equal to or less than this number of times will be counted and displayed in the list.
- Includes an option to reference a MeshRenderer in the scene that uses each material. This can be particularly useful for identifying where a material is being used, especially when there is only one instance.

## Usage

To use Unity Material Usage Analyzer, follow these steps:

1. Download the `MaterialUsageWindow.cs` script file from this repository.
2. Place the script in a folder named "Editor" in your Unity project's "Assets" directory.
3. Open the Unity editor and navigate to the "Window" tab on the menu bar.
4. Select the "Material Usage" option to open the Unity Material Usage Analyzer window.
5. With the Material Usage window open, you can click the "Get Material Usage" button to analyze the current scene and update the material usage information.

You can adjust the "Usage Threshold" to limit the display and count of materials to those used a certain number of times or less. Tick the "Limit Count" checkbox to enable this feature. If the "Ping Meshes" checkbox is enabled, a second column will appear in the list providing a reference to one of the MeshRenderers using each material. This can be used to located a mesh using the material for reference.

## Contributing

Please feel free to fork, raise issues, or submit pull requests. All feedback or contributions are appreciated.

## License

Unity Material Usage Analyzer is available under the MIT license. See the LICENSE file for more info.
