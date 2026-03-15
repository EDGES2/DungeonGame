shader_type canvas_item;

// This receives the sub-pixel fraction from our camera script
uniform vec2 cam_offset = vec2(0.0, 0.0);

void vertex() {
    // Shift the entire screen's vertices in the opposite direction 
    // to compensate for the snapped camera.
    VERTEX -= cam_offset; 
}