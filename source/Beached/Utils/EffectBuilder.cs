using Klei.AI;
using System.Collections.Generic;

namespace Beached.Utils
{
    public class EffectBuilder
    {
        private readonly string ID;
        private string name;
        private string description;
        private readonly float duration;
        private bool triggerFloatingText;
        private bool showInUI;
        private readonly bool isBad;
        private List<AttributeModifier> modifiers;
        private Emote emote;
        private float emoteCooldown;
        private string customIcon;
        private List<Reactable.ReactablePrecondition> emotePreconditions;

        public EffectBuilder(string ID, float duration, bool isBad)
        {
            name = Strings.Get("STRINGS.EFFECTS." + ID.ToUpper() + ".NAME");
            description = Strings.Get("STRINGS.EFFECTS." + ID.ToUpper() + ".DESC");
            triggerFloatingText = true;
            showInUI = true;
            this.duration = duration;
            this.isBad = isBad;
            this.ID = ID;
            customIcon = "";
        }

        public EffectBuilder Name(string name)
        {
            this.name = name;

            return this;
        }

        public EffectBuilder Description(string description)
        {
            this.description = description;

            return this;
        }

        public EffectBuilder Modifier(string id, float value, bool isMultiplier = false)
        {
            modifiers = modifiers ?? new List<AttributeModifier>();
            modifiers.Add(new AttributeModifier(id, value, name, is_multiplier: isMultiplier));

            return this;
        }

        public EffectBuilder Emote(Emote emote, float emoteCooldown)
        {
            this.emote = emote;
            this.emoteCooldown = emoteCooldown;

            return this;
        }

        public EffectBuilder EmotePrecondition(Reactable.ReactablePrecondition condition)
        {
            emotePreconditions = emotePreconditions ?? new List<Reactable.ReactablePrecondition>();
            emotePreconditions.Add(condition);

            return this;
        }

        public EffectBuilder HideFloatingText()
        {
            triggerFloatingText = false;

            return this;
        }

        public EffectBuilder HideInUI()
        {
            showInUI = false;

            return this;
        }

        public Effect Add(ModifierSet set)
        {
            var effect = new Effect(ID, name, description, duration, showInUI, triggerFloatingText, isBad, emote, emoteCooldown, custom_icon: customIcon);

            if (modifiers != null)
            {
                effect.SelfModifiers = modifiers;
            }

            if (emotePreconditions != null)
            {
                effect.emotePreconditions = emotePreconditions;
            }

            set.effects.Add(effect);

            return effect;
        }
    }
}
