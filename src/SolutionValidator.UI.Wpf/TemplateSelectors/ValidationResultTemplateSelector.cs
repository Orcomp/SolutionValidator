namespace SolutionValidator.Converters
{
    using System.Windows;
    using System.Windows.Controls;
    using Common;

    class ValidationResultTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;

            if (element != null && item != null && item is ValidationMessage)
            {
                var validationMessage = item as ValidationMessage;

                if (validationMessage.ResultLevel == ResultLevel.Error || validationMessage.ResultLevel == ResultLevel.NotPassed)
                {
                    return element.FindResource("ErrorTemplate") as DataTemplate;
                }

                if (validationMessage.ResultLevel == ResultLevel.Warning)
                {
                    return element.FindResource("WarningTemplate") as DataTemplate;
                }

                if (validationMessage.ResultLevel == ResultLevel.Passed)
                {
                    return element.FindResource("WarningTemplate") as DataTemplate;
                }

                if (validationMessage.ResultLevel == ResultLevel.Info)
                {
                    return element.FindResource("InfoTemplate") as DataTemplate;
                }
            }

            return null;
        }
    }
}
