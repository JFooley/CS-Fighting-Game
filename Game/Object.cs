using SFML.Graphics;
using SFML.System;

namespace Object_Space {
    public class Object {
        // Informações básicas
        public Vector2f Position = new Vector2f(0, 0);
        public float size_ratio = 1.0f;
        public int facing = 1;

        // Aux
        private DateTime timer;
        private double current_time => (DateTime.Now - this.timer).TotalSeconds;

        // Estado
        public bool active = true;
        public bool animate = true;
        public bool behave = true;
        public bool render = true;
        public bool remove = false;

        public Object() {}

        // Métodos de atualização e comportamento
        public virtual void Update() {
            if (!this.active) return;
        }
        public virtual void DoBehave() {
            if (!this.behave) return;
        }
        public virtual void DoRender(RenderWindow window, bool drawHitboxes = false) {
            if (!this.render) return;
        }
        public virtual void DoAnimate() {
            if (!this.animate) return;
        }

        // Metodo auxiliar
        public void ResetTimer() {
            this.timer = DateTime.Now;
        }
        public bool CheckTimer(double elapsed_time) {
            return elapsed_time <= this.current_time;
        }
    

        public virtual void Load(string Path) {}
        public virtual void Unload() {}
    }
}
