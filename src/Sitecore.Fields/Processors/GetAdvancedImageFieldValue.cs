using Sitecore.Collections;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Xml.Xsl;
using Sitecore.Pipelines.RenderField;

namespace Sitecore.Framework.Fields.Processors
{
    /// <summary>
    /// Renders the image from <see cref="P:Sitecore.Pipelines.RenderField.RenderFieldArgs.Item" /> into <see cref="P:Sitecore.Pipelines.RenderField.RenderFieldArgs.Result" />.
    /// </summary>
    public class GetAdvancedImageFieldValue
    {
        /// <summary>The name of the 'title' field in MediaItem.</summary>
        private static readonly string _TitleFieldName = "title";

        /// <summary>Gets the name of the 'title' field in MediaItem.</summary>
        /// <value>The name of the title field.</value>
        protected virtual string TitleFieldName
        {
            get
            {
                return _TitleFieldName;
            }
        }

        /// <summary>
        /// Renders an image in case arguments carry an image details.
        /// </summary>
        /// <param name="args">The arguments with rendering parameters (f.e. Item, fieldName).</param>
        public virtual void Process(RenderFieldArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));
            if (!IsImage(args))
                return;
            ImageRenderer renderer = CreateRenderer();
            ConfigureRenderer(args, renderer);
            SetRenderFieldResult(renderer.Render(), args);
        }

        /// <summary>
        /// Checks if <see cref="T:Sitecore.Pipelines.RenderField.RenderFieldArgs" /> carries image details.
        /// </summary>
        /// <param name="args">The rendering details.</param>
        /// <returns><c>true</c> if an image to be rendered;<c>false</c> otherwise.</returns>
        protected virtual bool IsImage(RenderFieldArgs args)
        {
            return args.FieldTypeKey == "advance Image";
        }

        /// <summary>Sets the rendering result into pipeline arguments.</summary>
        /// <param name="result">The result of rendering data specified in args.</param>
        /// <param name="args">The arguments that carry rendering result.</param>
        protected virtual void SetRenderFieldResult(RenderFieldResult result, RenderFieldArgs args)
        {
            args.Result.FirstPart = result.FirstPart;
            args.Result.LastPart = result.LastPart;
            args.WebEditParameters.AddRange(args.Parameters);
            args.DisableWebEditContentEditing = true;
            args.DisableWebEditFieldWrapping = true;
            args.WebEditClick = "return Sitecore.WebEdit.editControl($JavascriptParameters, 'webedit:chooseimage')";
        }

        /// <summary>
        /// Configures <paramref name="imageRenderer" /> with values from <paramref name="args" />.
        /// <para>Sets an item to be rendered, as well as additional parameters.</para>
        /// </summary>
        /// <param name="args"> Carries details of an item to be rendered.</param>
        /// <param name="imageRenderer">The renderer to render item specified in args.</param>
        protected virtual void ConfigureRenderer(RenderFieldArgs args, ImageRenderer imageRenderer)
        {
            Item itemToRender = args.Item;
            imageRenderer.Item = itemToRender;
            imageRenderer.FieldName = args.FieldName;
            imageRenderer.FieldValue = args.FieldValue;
            imageRenderer.Parameters = args.Parameters;
            if (itemToRender == null)
                return;
            imageRenderer.Parameters.Add("la", itemToRender.Language.Name);
            EnsureMediaItemTitle(args, itemToRender, imageRenderer);
        }

        /// <summary>Ensures title is rendered for image field.</summary>
        /// <param name="args"> Carries details of an item to be rendered.</param>
        /// <param name="itemToRender"> An item with image field to be rendered.</param>
        /// <param name="imageRenderer">The renderer to render item specified in args.</param>
        protected virtual void EnsureMediaItemTitle(RenderFieldArgs args, Item itemToRender, ImageRenderer imageRenderer)
        {
            if (!string.IsNullOrEmpty(args.Parameters[TitleFieldName]))
                return;
            Item innerImageItem = GetInnerImageItem(args, itemToRender);
            if (innerImageItem == null)
                return;
            Field field = innerImageItem.Fields[TitleFieldName];
            if (field == null)
                return;
            string str = field.Value;
            if (string.IsNullOrEmpty(str) || imageRenderer.Parameters == null)
                return;
            imageRenderer.Parameters.Add(TitleFieldName, str);
        }

        /// <summary>
        /// Gets the inner image that is to be rendered from <paramref name="itemToRender" />.
        /// </summary>
        /// <param name="args">The arguments specify what field to render.</param>
        /// <param name="itemToRender">The item to render.</param>
        /// <returns>An inner image item to be rendered ;<c>null</c> otherwise.</returns>
        protected virtual Item GetInnerImageItem(RenderFieldArgs args, Item itemToRender)
        {
            Field field = itemToRender.Fields[args.FieldName];
            if (field == null)
                return null;
            return new ImageField(field, args.FieldValue).MediaItem;
        }

        /// <summary>
        /// Creates the new instance of the <see cref="T:Sitecore.Xml.Xsl.ImageRenderer" /> class that will do the rendering part.
        /// </summary>
        /// <returns>The renderer.</returns>
        protected virtual ImageRenderer CreateRenderer()
        {
            return new ImageRenderer();
        }
    }
}
