using System.ComponentModel;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Processors;
using UnityEngine.InputSystem.Utilities;

#if UNITY_EDITOR
    [InitializeOnLoad]
#endif

[DisplayStringFormat("{modifier}+{negative}/{positive}")]
[DisplayName("Positive/Negative Binding With One Modifier")]
public class AxisWithOneModifierInputComposite : InputBindingComposite<float> {
    [InputControl(layout = "Button")] public int modifier;
    [InputControl(layout = "Axis")] public int negative = 0;
    [InputControl(layout = "Axis")] public int positive = 0;

    public bool overrideModifiersNeedToBePressedFirst;

    [Tooltip("Value to return when the negative side is fully actuated.")]
    public float minValue = -1;

    [Tooltip("Value to return when the positive side is fully actuated.")]
    public float maxValue = 1;

    [Tooltip("If both the positive and negative side are actuated, decides what value to return. 'Neither' (default) means that " +
        "the resulting value is the midpoint between min and max. 'Positive' means that max will be returned. 'Negative' means that " +
        "min will be returned.")]
    public WhichSideWins whichSideWins = WhichSideWins.Neither;

    public float midPoint => (maxValue + minValue) / 2;

    public override float ReadValue(ref InputBindingCompositeContext context) {
        if (!ModifierIsPressed(ref context))
            return default;

        var negativeValue = Mathf.Abs(context.ReadValue<float>(negative));
        var positiveValue = Mathf.Abs(context.ReadValue<float>(positive));

        var negativeIsActuated = negativeValue > Mathf.Epsilon;
        var positiveIsActuated = positiveValue > Mathf.Epsilon;

        if (negativeIsActuated == positiveIsActuated) {
            switch (whichSideWins) {
                case WhichSideWins.Negative:
                    positiveIsActuated = false;
                    break;

                case WhichSideWins.Positive:
                    negativeIsActuated = false;
                    break;

                case WhichSideWins.Neither:
                    return midPoint;
            }
        }

        var mid = midPoint;

        if (negativeIsActuated)
            return mid - (mid - minValue) * negativeValue;

        return mid + (maxValue - mid) * positiveValue;
    }

    private bool ModifierIsPressed(ref InputBindingCompositeContext context) {
        var modifierDown = context.ReadValueAsButton(modifier);

        if (modifierDown && !overrideModifiersNeedToBePressedFirst) {
            var timestampP = context.GetPressTime(positive);
            var timestampN = context.GetPressTime(negative);
            var timestampM = context.GetPressTime(modifier);

            return (timestampM <= timestampP) || (timestampM <= timestampN);
        }

        return modifierDown;
    }

    public override float EvaluateMagnitude(ref InputBindingCompositeContext context) {
        var value = ReadValue(ref context);
        if (value < midPoint) {
            value = Mathf.Abs(value - midPoint);
            return NormalizeProcessor.Normalize(value, 0, Mathf.Abs(minValue), 0);
        }

        value = Mathf.Abs(value - midPoint);
        return NormalizeProcessor.Normalize(value, 0, Mathf.Abs(maxValue), 0);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1717:OnlyFlagsEnumsShouldHavePluralNames", Justification = "False positive: `Wins` is not a plural form.")]
    public enum WhichSideWins {
        Neither = 0,
        Positive = 1,
        Negative = 2,
    }

    static AxisWithOneModifierInputComposite() {
        InputSystem.RegisterBindingComposite<AxisWithOneModifierInputComposite>();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() { }
}