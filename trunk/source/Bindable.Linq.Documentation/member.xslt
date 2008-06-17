<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet 
    version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    exclude-result-prefixes="msxsl">
	
    <!-- assumes minimal whitespace formatting in XML -->
    <xsl:output omit-xml-declaration="yes" method="text" indent="no" encoding="utf-8"/>
	<xsl:strip-space elements="page summary declaration returns" />
	
	<xsl:template match="/member">
		<xsl:text>!!! Summary&#10;</xsl:text>
		<xsl:apply-templates select="summary" />
		
		<xsl:text>!!!! Declaration&#10;{{&#10;</xsl:text>
		<xsl:apply-templates select="declaration" />
		<xsl:text>&#10;}}&#10;</xsl:text>
		
		<xsl:if test="returns != ''">
			<xsl:text>!!!! Returns&#10;</xsl:text>
			<xsl:apply-templates select="returns" />
		</xsl:if>
		
		<xsl:text>!!!! Parameters&#10;</xsl:text>
		<xsl:apply-templates select="param | typeparam" />
		
		<xsl:if test="exception != ''">
			<xsl:text>!!!! Exceptions&#10;</xsl:text>
			<xsl:apply-templates select="exception" />
		</xsl:if>

		<xsl:if test="example != ''">
			<xsl:text>!!! Example&#10;</xsl:text>
			<xsl:apply-templates select="example" />
		</xsl:if>

		<xsl:if test="differences != ''">
			<xsl:text>!!! Differences with LINQ to Objects&#10;</xsl:text>
			<xsl:apply-templates select="differences" />
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="see">
		<!-- template for wiki links -->
		<xsl:text>[</xsl:text>
		<xsl:value-of select="@cref"/>
		<xsl:text>]</xsl:text>
	</xsl:template>

    <xsl:template match="*">
		<!-- everything else -->
        <xsl:value-of select="."/>
    </xsl:template>
    
	<xsl:template match="summary | declaration | returns">
		<xsl:apply-templates select="node() | *"/>
		<xsl:text>&#10;</xsl:text>
	</xsl:template>

	<xsl:template match="param | typeparam">
		<!-- parameter list members -->
		<xsl:text>* *</xsl:text>
		<xsl:value-of select="@name"/>
		<xsl:text>* </xsl:text>
		<xsl:apply-templates select="node() | *"/>
		<xsl:text>&#10;</xsl:text>
	</xsl:template>

	<xsl:template match="exception">
		<xsl:text>* *</xsl:text>
		<xsl:value-of select="@cref"/>
		<xsl:text>* </xsl:text>
		<xsl:apply-templates select="node() | *"/>
		<xsl:text>&#10;</xsl:text>
	</xsl:template>

	<xsl:template match="paramref">
		<!-- parameter references inside of exception tag-->
		<xsl:text>_</xsl:text>
		<xsl:value-of select="@name" />
		<xsl:text>_</xsl:text>
	</xsl:template>

    <xsl:template match="example">
        <xsl:text>{{&#10;</xsl:text>
		<xsl:value-of select="node() | *"/>
		<xsl:text>&#10;}}&#10;</xsl:text>
    </xsl:template>
	
    <xsl:template match="differences">
		<xsl:apply-templates select="node() | *"/>
    </xsl:template>
</xsl:stylesheet>
