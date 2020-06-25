# Sitecore Fields

# Advance Image Sitecore
An image field with focus cropping option. Allows content authors to set focal point on images and cropping is done based on defined axes.

Package installs the field definition in Core database and setups templates and default thumbnails in Master database. This package also contains configuration and assembly files.

Complete installation instructions are available at:
https://inverseproportion.wordpress.com/2017/04/12/image-field-with-cropper-in-sitecore

Advance Image field has following dependencies:
- Sitecore.Kernel
- Sitecore.Mvc
- ImageProcessor
- System.Drawing
- System.Web

# Slider
A slider field allowing author to select value from within the defined range.

# Support for Sitecore Experience Editor.
Example for Razor view

<pre><code>
@if (Sitecore.Context.PageMode.IsExperienceEditor)
{
  &lt;p>@Html.Sitecore().Field("Image", new { w = 640, h = 360 })&lt;/p>
} else
{
  &lt;picture class="picture-component responsive lazy whatever">
   @Html.Sitecore().AdvancedImageField("Image", Sitecore.Context.Item,360, 640)
  &lt;/picture>
}
</code></pre>

## Download
pick  the latest release build here ![/Sitecore Package](https://github.com/jbluemink/sitecore-fields/tree/master/Sitecore%20Package)
