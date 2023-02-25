#include<glad/glad.h>

#include<GLFW/glfw3.h>

#include<glm/glm.hpp>
#include<glm/gtc/matrix_transform.hpp>
#include<glm/gtc/type_ptr.hpp>

#include<iostream>

#include"../unitlty/shader.h"


#define STB_IMAGE_IMPLEMENTATION
#include"../unitlty/std_image.h"

void framebuffer_size_callback(GLFWwindow* window, int width, int height);
void processInput(GLFWwindow* window);
unsigned int makeTexture(const char* filepath);


//float vertices[] = {
//	// position				// colors				// texture coords
//	 0.5f,  0.5f, 0.0f,    1.0f, 0.0f, 0.0f,		0.5f, 0.5f, //top right
//	 0.5f, -0.5f, 0.0f,    0.0f, 1.0f, 0.0f,        0.5f, 0.49f, //bottom right
//	-0.5f, -0.5f, 0.0f,    0.0f, 0.0f, 1.0f,        0.49f, 0.49f, //bottom left
//	-0.5f,  0.5f, 0.0f,    1.0f, 1.0f, 0.0f,        0.49f, 0.5f  //top left
//};
float vertices[] = {
	// position						// texture coords
	 0.5f,  0.5f, 0.0f,    	1.0f, 1.0f, //top right
	 0.5f, -0.5f, 0.0f,     1.0f, 0.0f, //bottom right
	-0.5f, -0.5f, 0.0f,     0.0f, 0.0f, //bottom left
	-0.5f,  0.5f, 0.0f,     0.0f, 1.0f  //top left
};

unsigned int indices[] = {
	0,1,3,
	1,2,3
};

float texCoords[] = {
	0.0f, 0.0f,
	1.0f, 0.0f,
	0.5f, 1.0f
};

float u = 0.2;

int main()
{


	glfwInit();
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

	GLFWwindow* window = glfwCreateWindow(800, 600, "LearnOpenGL", NULL, NULL);
	if (window == NULL)
	{
		std::cout << "Failed to create GLFW window" << std::endl;
		glfwTerminate();
		return -1;
	}
	glfwMakeContextCurrent(window);

	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
	{
		std::cout << "Failed to initialize GLAD" << std::endl;
		return -1;
	}

	glViewport(0, 0, 800, 600);
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);

	int nrAttributes;
	glGetIntegerv(GL_MAX_VERTEX_ATTRIBS, &nrAttributes);
	std::cout << "Maximum nr of vertex attributes supported: " << nrAttributes << std::endl;



	float borderColor[] = { 1.0f,1.0f,0.0f,1.0f };
	glTexParameterfv(GL_TEXTURE_2D, GL_TEXTURE_BORDER_COLOR, borderColor);

	//--------------make shader-----------------------
	//Shader shader0("../shaders/1_3_TexturesShader_1.vs", "../shaders/1_3_TexturesShader_1.fs");
	Shader shader1("../shaders/1_4_1.vs", "../shaders/1_4_1.fs");
	//--------------------make mesh------------------


	unsigned int VAO;
	glGenVertexArrays(1, &VAO);
	glBindVertexArray(VAO);

	unsigned int VBO;
	glGenBuffers(1, &VBO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

	unsigned int EBO;
	glGenBuffers(1, &EBO);

	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);

	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)0);
	glEnableVertexAttribArray(0);
	//glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)(3 * sizeof(float)));
	//glEnableVertexAttribArray(1);
	glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 5 * sizeof(float), (void*)(3 * sizeof(float)));
	glEnableVertexAttribArray(1);


	glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);
	glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);

	//-----------------texture-----------------------
	unsigned int texture1, texture2;
	texture1 = makeTexture("../textures/container.jpg");
	texture2 = makeTexture("../textures/awesomeface.png");

	glActiveTexture(GL_TEXTURE0);
	glBindTexture(GL_TEXTURE_2D, texture1);
	glActiveTexture(GL_TEXTURE1);
	glBindTexture(GL_TEXTURE_2D, texture2);
	shader1.use();
	glUniform1i(glGetUniformLocation(shader1.ID, "texture1"), 0);
	shader1.setInt("texture2", 1);






	while (!glfwWindowShouldClose(window))
	{
		processInput(window);

		glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT);

		//glBindTexture(GL_TEXTURE_2D, texture1);

		glm::mat4 trans = glm::mat4(1.0f);
		
		
		trans = glm::translate(trans, glm::vec3(0.5f, -0.5f, 0.0f));
		trans = glm::rotate(trans, (float)glfwGetTime(), glm::vec3(0.0f, 0.0f, 1.0f));
		trans = glm::scale(trans, glm::vec3(0.5));

		glm::mat4 trans1 = glm::mat4(1.0f);
		trans1 = glm::translate(trans1, glm::vec3(-0.25, 0.25f, 0.0f));
		trans1 = glm::scale(trans1, glm::vec3((float)sin(glfwGetTime())/2));


		shader1.use();
		unsigned int transformLoc = glGetUniformLocation(shader1.ID, "transform");
		glUniformMatrix4fv(transformLoc, 1, GL_FALSE, glm::value_ptr(trans));
		shader1.setFloat("u", u);
		glBindVertexArray(VAO);
		glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);
		shader1.setMat4("transform", trans1);
		glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);



		glfwSwapBuffers(window);
		glfwPollEvents();
	}



	glfwTerminate();
	return 0;
}

void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
	glViewport(0, 0, width, height);
}

void processInput(GLFWwindow* window)
{
	if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
		glfwSetWindowShouldClose(window, true);
	if (glfwGetKey(window, GLFW_KEY_UP) == GLFW_PRESS)
		u = std::min(u + 0.01, 1.0);
	if (glfwGetKey(window, GLFW_KEY_DOWN) == GLFW_PRESS)
		u = std::max(u - 0.01, 0.0);

}

unsigned int makeTexture(const char* filepath)
{
	unsigned int texture;
	glGenTextures(1, &texture);
	glBindTexture(GL_TEXTURE_2D, texture);

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST_MIPMAP_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);

	stbi_set_flip_vertically_on_load(true);

	int width, height, nrChannels;
	unsigned char* data = stbi_load(filepath, &width, &height, &nrChannels, 0);
	if (data)
	{
		if (nrChannels == 3) {
			glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, data);
		}
		else if (nrChannels == 4)
		{
			glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGBA, GL_UNSIGNED_BYTE, data);
		}

		glGenerateMipmap(GL_TEXTURE_2D);
	}
	else
	{
		std::cout << "Failed to load texture" << std::endl;
	}
	stbi_image_free(data);
	return texture;
}