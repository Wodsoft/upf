using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Controls
{
    public class Grid : Panel
    {
        private ColumnDefinitionCollection _columns;
        private RowDefinitionCollection _rows;

        public Grid()
        {
            _columns = new ColumnDefinitionCollection(this);
            _rows = new RowDefinitionCollection(this);
        }

        #region Initialize

        public override void EndInit()
        {
            CalculateColumn();
            CalculateRow();
            base.EndInit();
        }

        #endregion

        #region Properties

        public ColumnDefinitionCollection ColumnDefinitions => _columns;

        public RowDefinitionCollection RowDefinitions => _rows;

        public static readonly DependencyProperty ShowGridLinesProperty =
            DependencyProperty.Register(
              "ShowGridLines",
              typeof(bool),
              typeof(Grid),
              new FrameworkPropertyMetadata(
                      false,
                      FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ColumnProperty =
            DependencyProperty.RegisterAttached(
              "Column",
              typeof(int),
              typeof(Grid),
              new FrameworkPropertyMetadata(
                      0,
                      new PropertyChangedCallback(OnCellAttachedPropertyChanged)),
              new ValidateValueCallback(IsIntValueNotNegative));
        public static int GetColumn(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (int)element.GetValue(ColumnProperty)!;
        }
        public static void SetColumn(UIElement element, int value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(ColumnProperty, value);
        }

        public static readonly DependencyProperty ColumnSpanProperty =
            DependencyProperty.RegisterAttached(
              "ColumnSpan",
              typeof(int),
              typeof(Grid),
              new FrameworkPropertyMetadata(
                      1,
                      new PropertyChangedCallback(OnCellAttachedPropertyChanged)),
              new ValidateValueCallback(IsIntValueGreaterThanZero));
        public static int GetColumnSpan(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (int)element.GetValue(ColumnSpanProperty)!;
        }
        public static void SetColumnSpan(UIElement element, int value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(ColumnSpanProperty, value);
        }

        public static readonly DependencyProperty RowProperty =
        DependencyProperty.RegisterAttached(
              "Row",
              typeof(int),
              typeof(Grid),
              new FrameworkPropertyMetadata(
                      0,
                      new PropertyChangedCallback(OnCellAttachedPropertyChanged)),
              new ValidateValueCallback(IsIntValueNotNegative));
        public static int GetRow(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (int)element.GetValue(RowProperty)!;
        }
        public static void SetRow(UIElement element, int value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(RowProperty, value);
        }

        public static readonly DependencyProperty RowSpanProperty =
            DependencyProperty.RegisterAttached(
              "RowSpan",
              typeof(int),
              typeof(Grid),
              new FrameworkPropertyMetadata(
                      1,
                      new PropertyChangedCallback(OnCellAttachedPropertyChanged)),
              new ValidateValueCallback(IsIntValueGreaterThanZero));
        public static int GetRowSpan(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return (int)element.GetValue(RowSpanProperty)!;
        }
        public static void SetRowSpan(UIElement element, int value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(RowSpanProperty, value);
        }

        private static void OnCellAttachedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Visual? child = d as Visual;
            if (child != null)
            {
                Grid? grid = VisualTreeHelper.GetParent(child) as Grid;
                if (grid != null)
                {
                    grid.InvalidateMeasure();
                }
            }
        }

        private static bool IsIntValueNotNegative(object? value)
        {
            if (value is int v)
                return v >= 0;
            return false;
        }

        private static bool IsIntValueGreaterThanZero(object? value)
        {
            if (value is int v)
                return v > 0;
            return false;
        }

        #endregion

        #region Layout

        private float _minWidth, _minHeight;

        internal void CalculateColumn()
        {
            var total = 0f;
            _minWidth = 0f;
            for (int i = 0; i < _columns.Count; i++)
            {
                var c = _columns[i];
                _minWidth += c.MinWidth;
                if (c.Width.GridUnitType == GridUnitType.Star)
                    total += c.Width.Value;
                if (c.Width.GridUnitType == GridUnitType.Pixel)
                    _minHeight += MathF.Max(c.MinWidth, c.Width.Value);
                else
                    _minHeight += c.MinWidth;
            }
            if (total != 0f)
            {
                for (int i = 0; i < _columns.Count; i++)
                {
                    var c = _columns[i];
                    if (c.Width.GridUnitType == GridUnitType.Star)
                        c.Percent = c.Width.Value / total;
                }
            }
        }

        internal void CalculateRow()
        {
            var total = 0f;
            _minHeight = 0f;
            for (int i = 0; i < _rows.Count; i++)
            {
                var r = _rows[i];
                if (r.Height.GridUnitType == GridUnitType.Star)
                    total += r.Height.Value;
                if (r.Height.GridUnitType == GridUnitType.Pixel)
                    _minHeight += MathF.Max(r.MinHeight, r.Height.Value);
                else
                    _minHeight += r.MinHeight;
            }
            if (total != 0f)
            {
                for (int i = 0; i < _rows.Count; i++)
                {
                    var r = _rows[i];
                    if (r.Height.GridUnitType == GridUnitType.Star)
                        r.Percent = r.Height.Value / total;
                }
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count != 0)
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    var child = Children[i]!;
                    var column = GetColumn(child);
                    var columnSpan = GetColumnSpan(child);
                    var row = GetRow(child);
                    var rowSpan = GetRowSpan(child);

                    var x = _columns[column].Offset;
                    var y = _rows[row].Offset;
                    float width = _columns[column].ActualWidth, height = _rows[row].ActualHeight;
                    for (int ii = 1; ii < columnSpan; ii++)
                        width += _columns[column + ii].ActualWidth;
                    for (int ii = 1; ii < rowSpan; ii++)
                        height += _rows[row + ii].ActualHeight;
                    child.Arrange(new Rect(x, y, width, height));
                }
            }
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            //Calculate column width
            var availableWidth = availableSize.Width;
            var availableHeight = availableSize.Height;
            //Calculate column and row cell of pixel unit firstly
            for (int i = 0; i < _columns.Count; i++)
            {
                var column = _columns[i];
                if (FloatUtil.IsZero(availableWidth))
                {
                    column.ActualWidth = 0f;
                    continue;
                }
                if (column.Width.IsAbsolute)
                {
                    var width = MathF.Min(MathF.Max(column.Width.Value, column.MinWidth), column.MaxWidth);
                    if (width > availableWidth)
                    {
                        column.ActualWidth = width;
                        availableWidth -= width;
                    }
                    else
                    {
                        column.ActualWidth = availableWidth;
                        availableWidth = 0f;
                    }
                }
            }
            for (int i = 0; i < _rows.Count; i++)
            {
                var row = _rows[i];
                if (FloatUtil.IsZero(availableHeight))
                {
                    row.ActualHeight = 0f;
                    continue;
                }
                if (row.Height.IsAbsolute)
                {
                    var height = MathF.Min(MathF.Max(row.Height.Value, row.MinHeight), row.MaxHeight);
                    if (height > availableHeight)
                    {
                        row.ActualHeight = height;
                        availableHeight -= height;
                    }
                    else
                    {
                        row.ActualHeight = availableHeight;
                        availableHeight = 0f;
                    }
                }
            }
            ChildCell[]? children;
            if (Children.Count == 0)
            {
                children = null;
            }
            else
            {
                //Calculate column and row cell of auto
                children = new ChildCell[Children.Count];
                for (int i = 0; i < Children.Count; i++)
                {
                    var child = Children[i]!;
                    var column = GetColumn(child);
                    var columnSpan = GetColumnSpan(child);
                    var row = GetRow(child);
                    var rowSpan = GetRowSpan(child);
                    if (column + columnSpan > _columns.Count)
                        columnSpan = _columns.Count - column;
                    if (row + rowSpan > _rows.Count)
                        rowSpan = _rows.Count - row;
                    children[i] = new ChildCell
                    {
                        Column = column,
                        ColumnSpan = columnSpan,
                        Row = row,
                        RowSpan = rowSpan,
                        IsAutoColumn = _columns[column].Width.IsAuto,
                        IsAutoRow = _rows[row].Height.IsAuto,
                        Element = child
                    };
                }
                Array.Sort(children, new ChildCellComparer());

                //Measure children size to get max size for each auto cell
                for (int i = 0; i < children.Length; i++)
                {
                    var child = children[i];
                    if (child.ColumnSpan != 1 || child.RowSpan != 1)
                        continue;
                    if (child.IsAutoColumn || child.IsAutoRow)
                    {
                        var column = _columns[child.Column];
                        var row = _rows[child.Row];
                        //float width, height;
                        //if (column.Width.IsAbsolute)
                        //    width = column.ActualWidth;
                        //else
                        //    width = MathF.Min(column.MaxWidth, column.ActualWidth + availableWidth);
                        //if (row.Height.IsAbsolute)
                        //    height = row.ActualHeight;
                        //else
                        //    height = MathF.Min(row.MaxHeight, row.ActualHeight + availableHeight);
                        ////no more space, skip
                        //if (FloatUtil.IsZero(width) && FloatUtil.IsZero(height))
                        //    break;
                        //child.Element.Measure(new Size(width, height));
                        child.Element.Measure(new Size());
                        var desiredSize = child.Element.DesiredSize;
                        if (child.IsAutoColumn && desiredSize.Width > column.ActualWidth)
                        {
                            availableWidth -= desiredSize.Width - column.ActualWidth;
                            column.ActualWidth = desiredSize.Width;
                        }
                        if (child.IsAutoRow && desiredSize.Height > row.ActualHeight)
                        {
                            availableHeight -= desiredSize.Height - row.ActualHeight;
                            row.ActualHeight = desiredSize.Height;
                        }
                    }
                }
            }

            float columnOffset = 0f;
            bool haveAvaliableWidth = !FloatUtil.IsZero(availableWidth);
            bool haveAvaliableHeight = !FloatUtil.IsZero(availableHeight);
            for (int i = 0; i < _columns.Count; i++)
            {
                var column = _columns[i];
                //Calculate column of star if there have available width
                if (haveAvaliableWidth && column.Width.IsStar)
                    column.ActualWidth = availableWidth * column.Percent;
                column.Offset = columnOffset;
                columnOffset += column.ActualWidth;
            }
            float rowOffset = 0f;
            for (int i = 0; i < _rows.Count; i++)
            {
                var row = _rows[i];
                //Calculate row of star if there have avaliable height
                if (haveAvaliableHeight && row.Height.IsStar)
                    row.ActualHeight = availableHeight * row.Percent;
                row.Offset = rowOffset;
                rowOffset += row.ActualHeight;
            }

            //Measure children
            if (children != null)
            {
                for (int i = 0; i < children.Length; i++)
                {
                    var child = children[i];
                    ////If in a auto cell, don't need measure again
                    //if (child.IsAutoColumn && child.IsAutoRow && child.ColumnSpan == 1 && child.RowSpan == 1)
                    //    continue;
                    float width = _columns[child.Column].ActualWidth, height = _rows[child.Row].ActualHeight;
                    for (int ii = 1; ii < child.ColumnSpan; ii++)
                        width += _columns[child.Column + ii].ActualWidth;
                    for (int ii = 1; ii < child.RowSpan; ii++)
                        height += _rows[child.Row + ii].ActualHeight;
                    child.Element.Measure(new Size(width, height));
                }
            }

            return new Size(columnOffset, rowOffset);
        }

        private struct ChildCell
        {
            public int Column, ColumnSpan, Row, RowSpan;
            public bool IsAutoColumn, IsAutoRow;
            public bool FixedSize;
            public UIElement Element;
        }

        private class ChildCellComparer : IComparer<ChildCell>
        {
            public int Compare(ChildCell x, ChildCell y)
            {
                var xValue = Math.Min(x.Column, x.Row);
                var yValue = Math.Min(y.Column, y.Row);
                if (xValue == yValue)
                {
                    return 0;
                    //int xSpan, ySpan;
                    //if (x.Column == x.Row)
                    //    xSpan = Math.Max(x.ColumnSpan, x.RowSpan);
                    //else if (x.Column < x.Row)
                    //    xSpan = x.ColumnSpan;
                    //else
                    //    xSpan = x.RowSpan;
                    //if (y.Column == y.Row)
                    //    ySpan = Math.Max(y.ColumnSpan, y.RowSpan);
                    //else if (y.Column < y.Row)
                    //    ySpan = y.ColumnSpan;
                    //else
                    //    ySpan = y.RowSpan;
                    //if (xSpan == ySpan)
                    //    return 0;
                    //else if (xSpan < ySpan)
                    //    return -1;
                    //else
                    //    return 1;
                }
                else if (xValue < yValue)
                    return -1;
                else
                    return 1;
            }
        }

        #endregion
    }
}
